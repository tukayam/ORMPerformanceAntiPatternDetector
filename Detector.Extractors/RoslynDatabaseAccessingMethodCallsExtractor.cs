using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using Microsoft.CodeAnalysis;
using Detector.Extractors.DatabaseEntities;
using Detector.Extractors.Base;
using Detector.Models.ORM;

namespace Detector.Extractors
{
    public sealed class RoslynDatabaseAccessingMethodCallsExtractor : CSharpSyntaxWalker, DatabaseAccessingMethodCallsExtractor<LINQToSQL>
    {
        public List<DatabaseAccessingMethodCallStatement<LINQToSQL>> DatabaseAccessingMethodCalls { get; private set; }

        private readonly DatabaseEntityDeclarationsExtractor<LINQToSQL> _databaseEntityDeclarationsExtractor;
        private readonly SemanticModel _model;

        private Dictionary<QueryExpressionSyntax, DatabaseQuery<LINQToSQL>> _databaseQueries;

        public RoslynDatabaseAccessingMethodCallsExtractor(SemanticModel model
            , DatabaseEntityDeclarationsExtractor<LINQToSQL> databaseEntityDeclarationsExtractor)
            : base()
        {
            this._model = model;
            this._databaseEntityDeclarationsExtractor = databaseEntityDeclarationsExtractor;

            this._databaseQueries = new Dictionary<QueryExpressionSyntax, DatabaseQuery<LINQToSQL>>();
            this.DatabaseAccessingMethodCalls = new List<DatabaseAccessingMethodCallStatement<LINQToSQL>>();
        }

        public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            ObjectCreationExpressionSyntax sy = node.DescendantNodes().OfType<ObjectCreationExpressionSyntax>().FirstOrDefault();

            if (sy != null)
            {
                var ti = _model.GetTypeInfo(sy);
            }
            var ty = _model.GetTypeInfo(node);
            base.VisitVariableDeclaration(node);
        }

        /// <summary>
        /// Extracts DatabaseAccessingMethodCalls
        /// </summary>
        /// <param name="node"></param>
        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            //Invocation might be happening on the same line as the query expression
            IEnumerable<QueryExpressionSyntax> queryExpressionsInsideInvocationExpression = node.DescendantNodes().OfType<QueryExpressionSyntax>();

            foreach (QueryExpressionSyntax queryExpression in queryExpressionsInsideInvocationExpression)
            {
                this.VisitQueryExpression(queryExpression);
                if (_databaseQueries.ContainsKey(queryExpression))
                {
                    this.DatabaseAccessingMethodCalls.Add(new DatabaseAccessingMethodCallStatement<LINQToSQL>(_databaseQueries[queryExpression]));
                }
                break;
            }

            base.VisitInvocationExpression(node);
        }

        public override void VisitQueryExpression(QueryExpressionSyntax node)
        {
            if (!_databaseQueries.ContainsKey(node))
            {
                if (QueryIsDatabaseQuery(node))
                {
                    string queryText = node.GetText().ToString();
                    IEnumerable<DatabaseEntityDeclaration<LINQToSQL>> databaseEntityDeclarationsUsedInQuery = GetDatabaseEntityTypesInQuery(node);
                    DatabaseQuery<LINQToSQL> query = new DatabaseQuery<LINQToSQL>(queryText, databaseEntityDeclarationsUsedInQuery);
                    _databaseQueries.Add(node, query);
                }
            }
            base.VisitQueryExpression(node);
        }

        private bool QueryIsDatabaseQuery(QueryExpressionSyntax query)
        {
            foreach (var qeNode in query.DescendantNodes())
            {
                ITypeSymbol typeOfNode = _model.GetTypeInfo(qeNode).Type;
                if (typeOfNode != null
                    && _databaseEntityDeclarationsExtractor.EntityDeclarations.Any(e => typeOfNode.ToString().Contains(e.Name)))
                {
                    return true;
                }
            }

            return false;
        }

        private IEnumerable<DatabaseEntityDeclaration<LINQToSQL>> GetDatabaseEntityTypesInQuery(QueryExpressionSyntax query)
        {
            foreach (var qeNode in query.DescendantNodes())
            {
                ITypeSymbol typeOfNode = _model.GetTypeInfo(qeNode).Type;

                foreach (var entityType in _databaseEntityDeclarationsExtractor.EntityDeclarations.Where(e => typeOfNode.ToString().Contains(e.Name)))
                {
                    yield return new DatabaseEntityDeclaration<LINQToSQL>(entityType.Name);
                }
            }
        }
    }
}
