using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using Microsoft.CodeAnalysis;
using Detector.Extractors.DatabaseEntities;
using Detector.Extractors.Base;
using Detector.Models.ORM;
using Detector.Models.Others;

namespace Detector.Extractors
{
    public sealed class LINQToSQLORMSyntaxTreeExtractor : CSharpSyntaxWalker, DatabaseAccessingMethodCallsExtractor<LINQToSQL>
    {
        public List<DatabaseAccessingMethodCallStatement<LINQToSQL>> DatabaseAccessingMethodCalls { get; private set; }
        public List<DatabaseAccessingForeachLoopDeclaration<LINQToSQL>> DatabaseAccessingForeachLoopDeclarations { get; private set; }
        public List<DatabaseAccessingForLoopDeclaration<LINQToSQL>> DatabaseAccessingForLoopDeclarations { get; private set; }
      
        public List<ForEachLoopDeclaration> ForeachLoopDeclarations { get; private set; }
        public List<ForLoopDeclaration> ForLoopDeclarations { get; private set; }
        public List<WhileLoopDeclaration> WhileLoopDeclarations { get; private set; }
        public List<DoWhileLoopDeclaration> DoWhileLoopDeclarations { get; private set; }


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
            this.DatabaseAccessingForeachLoopDeclarations = new List<DatabaseAccessingForeachLoopDeclaration<LINQToSQL>>();
            this.DatabaseAccessingForLoopDeclarations = new List<DatabaseAccessingForLoopDeclaration<LINQToSQL>>();
         
            this.ForeachLoopDeclarations = new List<ForEachLoopDeclaration>();
            this.ForLoopDeclarations = new List<ForLoopDeclaration>();
            this.WhileLoopDeclarations = new List<WhileLoopDeclaration>();
            this.DoWhileLoopDeclarations = new List<DoWhileLoopDeclaration>();
        }

        public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            VisitChildren(node);

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
            VisitChildren(node);

            ExtractDatabaseAccessingMethodsThatIncludeAQuery(node);
            ExtractDatabaseAccessingMethodsThatInvokeAMethodOnAQueryVariable(node);
            base.VisitInvocationExpression(node);
        }

        public override void VisitQueryExpression(QueryExpressionSyntax node)
        {
            VisitChildren(node);

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

        public override void VisitForEachStatement(ForEachStatementSyntax node)
        {
            VisitChildren(node);
            DatabaseAccessingForeachLoopDeclaration<LINQToSQL> dbAccessingForEach = (from n in node.DescendantNodes().OfType<IdentifierNameSyntax>()
                                                                                     from v in _databaseQueryVariables.Keys
                                                                                     where n.Identifier.Text == v.DescendantNodes().OfType<VariableDeclaratorSyntax>().First().Identifier.Text
                                                                                     select new DatabaseAccessingForeachLoopDeclaration<LINQToSQL>()).FirstOrDefault();

            if (dbAccessingForEach != null)
            {
                DatabaseAccessingForeachLoopDeclarations.Add(dbAccessingForEach);
            }
            else
            {
                ForeachLoopDeclarations.Add(new ForEachLoopDeclaration());
            }

            base.VisitForEachStatement(node);
        }

        public override void VisitForStatement(ForStatementSyntax node)
        {
            VisitChildren(node);

            DatabaseAccessingForLoopDeclaration<LINQToSQL> dbAccessingFor = (from n in node.DescendantNodes().OfType<IdentifierNameSyntax>()
                                                                             from v in _databaseQueryVariables.Keys
                                                                             where n.Identifier.Text == v.DescendantNodes().OfType<VariableDeclaratorSyntax>().First().Identifier.Text
                                                                             select new DatabaseAccessingForLoopDeclaration<LINQToSQL>()).FirstOrDefault();

            if (dbAccessingFor != null)
            {
                DatabaseAccessingForLoopDeclarations.Add(dbAccessingFor);
            }
            else
            {
                ForLoopDeclarations.Add(new ForLoopDeclaration());
            }
            base.VisitForStatement(node);
        }

        public override void VisitWhileStatement(WhileStatementSyntax node)
        {
            VisitChildren(node);

            //DatabaseAccessingWhileLoopDeclaration<LINQToSQL> dbAccessingLoop = (from n in node.DescendantNodes().OfType<IdentifierNameSyntax>()
            //                                                                 from v in _databaseQueryVariables.Keys
            //                                                                 where n.Identifier.Text == v.DescendantNodes().OfType<VariableDeclaratorSyntax>().First().Identifier.Text
            //                                                                 select new DatabaseAccessingWhileLoopDeclaration<LINQToSQL>()).FirstOrDefault();

            //if (dbAccessingLoop != null)
            //{
            //    DatabaseAccessingWhileLoopDeclarations.Add(dbAccessingLoop);
            //}
            //else
            //{
            WhileLoopDeclarations.Add(new WhileLoopDeclaration());
            //  }

            base.VisitWhileStatement(node);
        }

        public override void VisitDoStatement(DoStatementSyntax node)
        {
            VisitChildren(node);
            base.VisitDoStatement(node);
        }

        public void VisitChildren(SyntaxNode node)
        {
            IEnumerable<SyntaxNode> childNodes = node.ChildNodes();
            if (childNodes.Count() > 0)
            {
                foreach (var n in childNodes)
                {
                    this.Visit(n);
                }
            }
        }

        private void ExtractDatabaseAccessingMethodsThatIncludeAQuery(InvocationExpressionSyntax node)
        {
            var dbAccessingMethodCalls = (from q in node.DescendantNodes().OfType<QueryExpressionSyntax>()
                                          where _databaseQueries.ContainsKey(q)
                                          select new DatabaseAccessingMethodCallStatement<LINQToSQL>(_databaseQueries[q]));

            this.DatabaseAccessingMethodCalls.AddRange(dbAccessingMethodCalls.ToList());
        }

        private void ExtractDatabaseAccessingMethodsThatInvokeAMethodOnAQueryVariable(InvocationExpressionSyntax node)
        {
            var variableDeclarationSyntax = from n in node.DescendantNodes().OfType<IdentifierNameSyntax>()
                                            from v in _databaseQueryVariables.Keys
                                            where n.Identifier.Text == v.DescendantNodes().OfType<VariableDeclaratorSyntax>().First().Identifier.Text
                                            select v;

            if (variableDeclarationSyntax.FirstOrDefault() != null && variableDeclarationSyntax.Count() == 1)
            {
                this.DatabaseAccessingMethodCalls.Add(new DatabaseAccessingMethodCallStatement<LINQToSQL>(_databaseQueries[_databaseQueryVariables[variableDeclarationSyntax.First()]]));
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
