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
    public sealed class LINQToSQLORMSyntaxTreeExtractor : CSharpSyntaxWalker, DatabaseAccessingMethodCallsExtractor<LINQToSQL>
    {
        public List<DatabaseAccessingMethodCallStatement<LINQToSQL>> DatabaseAccessingMethodCalls { get; private set; }

        private readonly List<DatabaseEntityDeclaration<LINQToSQL>> _databaseEntityDeclarations;
        private readonly SemanticModel _model;

        private Dictionary<VariableDeclarationSyntax, QueryExpressionSyntax> _databaseQueryVariables;
        private Dictionary<QueryExpressionSyntax, DatabaseQuery<LINQToSQL>> _databaseQueries;

        public LINQToSQLORMSyntaxTreeExtractor(SemanticModel model
            , DatabaseEntityDeclarationsExtractor<LINQToSQL> databaseEntityDeclarationsExtractor)
            : base()
        {
            this._model = model;
            this._databaseEntityDeclarations = databaseEntityDeclarationsExtractor.EntityDeclarations.ToList();

            this._databaseQueryVariables = new Dictionary<VariableDeclarationSyntax, QueryExpressionSyntax>();
            this._databaseQueries = new Dictionary<QueryExpressionSyntax, DatabaseQuery<LINQToSQL>>();
            this.DatabaseAccessingMethodCalls = new List<DatabaseAccessingMethodCallStatement<LINQToSQL>>();
        }

        public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            this.Visit(node.DescendantNodes().First());

            foreach (var queryExp in node.DescendantNodes().OfType<QueryExpressionSyntax>())
            {
                if (QueryIsDatabaseQuery(queryExp) && !_databaseQueryVariables.ContainsKey(node))
                {
                    _databaseQueryVariables.Add(node, queryExp);
                }
            }
            base.VisitVariableDeclaration(node);
        }

        /// <summary>
        /// Extracts DatabaseAccessingMethodCalls
        /// </summary>
        /// <param name="node"></param>
        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            this.Visit(node.DescendantNodes().First());

            ExtractDatabaseAccessingMethodsThatIncludeAQuery(node);
            ExtractDatabaseAccessingMethodsThatInvokeAMethodOnAQueryVariable(node);
            base.VisitInvocationExpression(node);
        }

        private void ExtractDatabaseAccessingMethodsThatIncludeAQuery(InvocationExpressionSyntax node)
        {
            foreach (QueryExpressionSyntax queryExpression in node.DescendantNodes().OfType<QueryExpressionSyntax>())
            {
                if (_databaseQueries.ContainsKey(queryExpression))
                {
                    this.DatabaseAccessingMethodCalls.Add(new DatabaseAccessingMethodCallStatement<LINQToSQL>(_databaseQueries[queryExpression]));
                }
                break;
            }
        }

        private void ExtractDatabaseAccessingMethodsThatInvokeAMethodOnAQueryVariable(InvocationExpressionSyntax node)
        {
            //Invocation might be happening on an earlier defined query variable            
            var variableDeclarationSyntax = from n in node.DescendantNodes().OfType<IdentifierNameSyntax>()
                                            from v in _databaseQueryVariables.Keys
                                            where n.Identifier.Text == v.DescendantNodes().OfType<VariableDeclaratorSyntax>().First().Identifier.Text
                                            select v;

            if (variableDeclarationSyntax != null && variableDeclarationSyntax.Count() == 1)
            {
                this.DatabaseAccessingMethodCalls.Add(new DatabaseAccessingMethodCallStatement<LINQToSQL>(_databaseQueries[_databaseQueryVariables[variableDeclarationSyntax.First()]]));
            }
        }

        public override void VisitQueryExpression(QueryExpressionSyntax node)
        {
            this.Visit(node.DescendantNodes().First());
            if (!_databaseQueries.ContainsKey(node))
            {
                if (QueryIsDatabaseQuery(node))
                {
                    string queryText = node.GetText().ToString();
                    List<DatabaseEntityDeclaration<LINQToSQL>> databaseEntityDeclarationsUsedInQuery = GetDatabaseEntityTypesInQuery(node);
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
                    && _databaseEntityDeclarations.Any(e => typeOfNode.ToString().Contains(e.Name)))
                {
                    return true;
                }
            }

            return false;
        }

        private List<DatabaseEntityDeclaration<LINQToSQL>> GetDatabaseEntityTypesInQuery(QueryExpressionSyntax query)
        {
            List<DatabaseEntityDeclaration<LINQToSQL>> result = new List<DatabaseEntityDeclaration<LINQToSQL>>();
            foreach (var qeNode in query.DescendantNodes())
            {
                ITypeSymbol typeOfNode = _model.GetTypeInfo(qeNode).Type;
                if (typeOfNode != null)
                {
                    var entityDeclarationInQuery = _databaseEntityDeclarations.Where(e => typeOfNode.ToString().Contains(e.Name)).FirstOrDefault();
                    if (entityDeclarationInQuery != null && !result.Exists(e => e == entityDeclarationInQuery))
                    {
                        result.Add(entityDeclarationInQuery);
                    }
                }
            }

            return result;
        }
    }
}
