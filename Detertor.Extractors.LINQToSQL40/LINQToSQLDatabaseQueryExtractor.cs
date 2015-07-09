using Detector.Extractors.Base;
using Detector.Models.ORM;
using Detector.Models.Others;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Detector.Extractors.LINQToSQL40
{
    public class LINQToSQLDatabaseQueryExtractor : CSharpSyntaxWalker, DatabaseQueryExtractor<LINQToSQL>
    {
        private readonly ModelCollection<DatabaseEntityDeclaration<LINQToSQL>> _databaseEntityDeclarations;
        private readonly SemanticModel _model;

        private Dictionary<VariableDeclarationSyntax, QueryExpressionSyntax> _databaseQueryVariables;
        private Dictionary<QueryExpressionSyntax, DatabaseQuery<LINQToSQL>> _databaseQueries;

        public ModelCollection<DatabaseQuery<LINQToSQL>> DatabaseQueries
        {
            get
            {
                var queries = new ModelCollection<DatabaseQuery<LINQToSQL>>();
                foreach (var item in _databaseQueries.Values)
                {
                    queries.Add(item);
                }
                return queries;
            }
        }

        public LINQToSQLDatabaseQueryExtractor(SemanticModel model
            , ModelCollection<DatabaseEntityDeclaration<LINQToSQL>> databaseEntityDeclarations)
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
                    ModelCollection<DatabaseEntityDeclaration<LINQToSQL>> databaseEntityDeclarationsUsedInQuery = GetDatabaseEntityTypesInQuery(node);

                    var queryVariable = (from qv in _databaseQueryVariables
                                         where qv.Value == node
                                         select new DatabaseQueryVariable(qv.Key.Variables[0].Identifier.Text)).FirstOrDefault();

                    var query = new DatabaseQuery<LINQToSQL>(queryText, databaseEntityDeclarationsUsedInQuery, queryVariable);
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

        private ModelCollection<DatabaseEntityDeclaration<LINQToSQL>> GetDatabaseEntityTypesInQuery(QueryExpressionSyntax query)
        {
            var result = new ModelCollection<DatabaseEntityDeclaration<LINQToSQL>>();
            foreach (var qeNode in query.DescendantNodes())
            {
                ITypeSymbol typeOfNode = _model.GetTypeInfo(qeNode).Type;
                if (typeOfNode != null)
                {
                    var entityDeclarationInQuery = _databaseEntityDeclarations.Where(e => typeOfNode.ToString().Contains(e.Name)).FirstOrDefault();
                    if (entityDeclarationInQuery != null && !result.Any(e => e == entityDeclarationInQuery))
                    {
                        result.Add(entityDeclarationInQuery);
                    }
                }
            }

            return result;
        }
    }
}
