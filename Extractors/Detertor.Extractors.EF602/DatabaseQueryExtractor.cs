using Detector.Extractors.Base;
using Detector.Models.ORM;
using Detector.Models.Others;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Detector.Extractors.EF602
{
    public class DatabaseQueryExtractor : DatabaseQueryExtractor<EntityFramework>
    {
        private readonly ModelCollection<DatabaseEntityDeclaration<EntityFramework>> _databaseEntityDeclarations;
        private readonly SemanticModel _model;

        private Dictionary<VariableDeclarationSyntax, QueryExpressionSyntax> _databaseQueryVariables;
        private Dictionary<QueryExpressionSyntax, DatabaseQuery<EntityFramework>> _databaseQueries;

        public ModelCollection<DatabaseQuery<EntityFramework>> DatabaseQueries
        {
            get
            {
                var queries = new ModelCollection<DatabaseQuery<EntityFramework>>();
                foreach (var item in _databaseQueries.Values)
                {
                    queries.Add(item);
                }
                return queries;
            }
        }

        public DatabaseQueryExtractor(Context<EntityFramework> context)
            : base(context)
        {
            this._databaseQueryVariables = new Dictionary<VariableDeclarationSyntax, QueryExpressionSyntax>();
            this._databaseQueries = new Dictionary<QueryExpressionSyntax, DatabaseQuery<EntityFramework>>();
        }

        public void VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            foreach (var queryExp in node.DescendantNodes().OfType<QueryExpressionSyntax>())
            {
                if (QueryIsDatabaseQuery(queryExp) && !_databaseQueryVariables.ContainsKey(node))
                {
                    _databaseQueryVariables.Add(node, queryExp);
                }
            }
           
        }

        public void VisitQueryExpression(QueryExpressionSyntax node)
        {
            if (!_databaseQueries.ContainsKey(node))
            {
                if (QueryIsDatabaseQuery(node))
                {
                    string queryText = node.GetText().ToString();
                    ModelCollection<DatabaseEntityDeclaration<EntityFramework>> databaseEntityDeclarationsUsedInQuery = GetDatabaseEntityTypesInQuery(node);

                    var queryVariable = (from qv in _databaseQueryVariables
                                         where qv.Value == node
                                         select new DatabaseQueryVariable(qv.Key.Variables[0].Identifier.Text)).FirstOrDefault();

                    var query = new DatabaseQuery<EntityFramework>(queryText, databaseEntityDeclarationsUsedInQuery, queryVariable);
                    _databaseQueries.Add(node, query);
                }
            }
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

        private ModelCollection<DatabaseEntityDeclaration<EntityFramework>> GetDatabaseEntityTypesInQuery(QueryExpressionSyntax query)
        {
            var result = new ModelCollection<DatabaseEntityDeclaration<EntityFramework>>();
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
