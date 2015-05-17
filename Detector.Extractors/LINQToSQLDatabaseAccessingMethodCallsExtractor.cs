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
    public sealed class LINQToSQLDatabaseAccessingMethodCallsExtractor : CSharpSyntaxWalker, DatabaseAccessingMethodCallsExtractor<LINQToSQL>
    {
        public List<DatabaseAccessingMethodCallStatement<LINQToSQL>> DatabaseAccessingMethodCalls { get; private set; }

        private readonly DatabaseEntityDeclarationsExtractor<LINQToSQL> _databaseEntityDeclarationsExtractor;
        private readonly SemanticModel _model;

        private Dictionary<VariableDeclarationSyntax, QueryExpressionSyntax> _databaseQueryVariables;
        private Dictionary<QueryExpressionSyntax, DatabaseQuery<LINQToSQL>> _databaseQueries;

        public LINQToSQLDatabaseAccessingMethodCallsExtractor(SemanticModel model
            , DatabaseEntityDeclarationsExtractor<LINQToSQL> databaseEntityDeclarationsExtractor)
            : base()
        {
            this._model = model;
            this._databaseEntityDeclarationsExtractor = databaseEntityDeclarationsExtractor;

            this._databaseQueryVariables = new Dictionary<VariableDeclarationSyntax, QueryExpressionSyntax>();
            this._databaseQueries = new Dictionary<QueryExpressionSyntax, DatabaseQuery<LINQToSQL>>();
            this.DatabaseAccessingMethodCalls = new List<DatabaseAccessingMethodCallStatement<LINQToSQL>>();
        }

        public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
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
            ExtractDatabaseAccessingMethodsThatIncludeAQuery(node);
            ExtractDatabaseAccessingMethodsThatInvokeAMethodOnAQueryVariable(node);

            base.VisitInvocationExpression(node);
        }

        private void ExtractDatabaseAccessingMethodsThatInvokeAMethodOnAQueryVariable(InvocationExpressionSyntax node)
        {
            //Invocation might be happening on an earlier defined query variable            
            IEnumerable<MemberAccessExpressionSyntax> memberAccessExpressions = node.DescendantNodes().OfType<MemberAccessExpressionSyntax>();
            foreach (var item in memberAccessExpressions)
            {
                foreach (var identifierNameSyntax in item.DescendantNodes().OfType<IdentifierNameSyntax>())
                {
                    VariableDeclarationSyntax variableDeclarationSyntax = _databaseQueryVariables.Keys.Where(k=>k.DescendantTokens().Any(t => t == identifierNameSyntax.Identifier)).FirstOrDefault();
                    if (variableDeclarationSyntax != null)
                    {
                        this.DatabaseAccessingMethodCalls.Add(new DatabaseAccessingMethodCallStatement<LINQToSQL>(_databaseQueries[_databaseQueryVariables[variableDeclarationSyntax]]));
                    }
                }
            }
        }

        private void ExtractDatabaseAccessingMethodsThatIncludeAQuery(InvocationExpressionSyntax node)
        {
            //Invocation might be happening on the same line as the query expression
            IEnumerable<QueryExpressionSyntax> queryExpressions = node.DescendantNodes().OfType<QueryExpressionSyntax>();

            foreach (QueryExpressionSyntax queryExpression in queryExpressions)
            {
                this.VisitQueryExpression(queryExpression);
                if (_databaseQueries.ContainsKey(queryExpression))
                {
                    this.DatabaseAccessingMethodCalls.Add(new DatabaseAccessingMethodCallStatement<LINQToSQL>(_databaseQueries[queryExpression]));
                }
                break;
            }
        }

        public override void VisitQueryExpression(QueryExpressionSyntax node)
        {
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
                    && _databaseEntityDeclarationsExtractor.EntityDeclarations.Any(e => typeOfNode.ToString().Contains(e.Name)))
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
                    var entityDeclarationInQuery = _databaseEntityDeclarationsExtractor.EntityDeclarations.Where(e => typeOfNode.ToString().Contains(e.Name)).FirstOrDefault();
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
