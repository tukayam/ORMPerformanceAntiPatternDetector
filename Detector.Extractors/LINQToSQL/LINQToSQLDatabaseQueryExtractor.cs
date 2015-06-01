using Detector.Extractors.Base;
using Detector.Extractors.DatabaseEntities;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Detector.Extractors
{
    public class LINQToSQLDatabaseQueryExtractor : CSharpSyntaxWalker, DatabaseQueryExtractor<LINQToSQL>
    {
        private readonly List<DatabaseEntityDeclaration<LINQToSQL>> _databaseEntityDeclarations;
        private readonly SemanticModel _model;

        private Dictionary<VariableDeclarationSyntax, QueryExpressionSyntax> _databaseQueryVariables;
        private Dictionary<QueryExpressionSyntax, DatabaseQuery<LINQToSQL>> _databaseQueries;

        public IEnumerable<DatabaseQuery<LINQToSQL>> DatabaseQueries
        {
            get
            {
                return _databaseQueries.Values;
            }
        }

        public LINQToSQLDatabaseQueryExtractor(SemanticModel model
            , List<DatabaseEntityDeclaration<LINQToSQL>> databaseEntityDeclarations)
            : base()
        {
            this._model = model;
            this._databaseEntityDeclarations = databaseEntityDeclarations;

            this._databaseQueryVariables = new Dictionary<VariableDeclarationSyntax, QueryExpressionSyntax>();
            this._databaseQueries = new Dictionary<QueryExpressionSyntax, DatabaseQuery<LINQToSQL>>();
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
