using Detector.Extractors.Base;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using Detector.Extractors.Helpers;

namespace Detector.Extractors
{
    public class LINQToSQLDatabaseAccessingMethodCallExtractor : CSharpSyntaxWalker, DatabaseAccessingMethodCallExtractor<LINQToSQL>
    {
        public List<DatabaseAccessingMethodCallStatement<LINQToSQL>> DatabaseAccessingMethodCalls { get; private set; }

        public Dictionary<DatabaseAccessingMethodCallStatement<LINQToSQL>, SyntaxNode> DatabaseAccessingMethodCallsAndSyntaxNodes { get; private set; }

        private readonly List<DatabaseEntityDeclaration<LINQToSQL>> _databaseEntityDeclarations;
        private readonly List<DatabaseQuery<LINQToSQL>> _databaseQueries;

        private readonly SemanticModel _model;

        public LINQToSQLDatabaseAccessingMethodCallExtractor(SemanticModel model
            , List<DatabaseEntityDeclaration<LINQToSQL>> databaseEntityDeclarations
            , List<DatabaseQuery<LINQToSQL>> databaseQueries)
            : base()
        {
            this._model = model;
            this._databaseEntityDeclarations = databaseEntityDeclarations;

            this._databaseQueries = databaseQueries;

            this.DatabaseAccessingMethodCalls = new List<DatabaseAccessingMethodCallStatement<LINQToSQL>>();
            this.DatabaseAccessingMethodCallsAndSyntaxNodes = new Dictionary<DatabaseAccessingMethodCallStatement<LINQToSQL>, SyntaxNode>();
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            ExtractDatabaseAccessingMethodsThatIncludeAQuery(node);
            ExtractDatabaseAccessingMethodsThatInvokeAMethodOnAQueryVariable(node);
            base.VisitInvocationExpression(node);
        }

        private void ExtractDatabaseAccessingMethodsThatIncludeAQuery(InvocationExpressionSyntax node)
        {
            DatabaseAccessingMethodCallStatement<LINQToSQL> dbAccessingMethodCall =
                (from q in node.DescendantNodes().OfType<QueryExpressionSyntax>()
                 from dq in _databaseQueries
                 where dq.IsSameQueryAs(q)
                 select new DatabaseAccessingMethodCallStatement<LINQToSQL>(dq, node.GetCompilationInfo())).FirstOrDefault();

            if (dbAccessingMethodCall != null)
            {
                this.DatabaseAccessingMethodCalls.Add(dbAccessingMethodCall);
                this.DatabaseAccessingMethodCallsAndSyntaxNodes.Add(dbAccessingMethodCall, node);
            }
        }

        private void ExtractDatabaseAccessingMethodsThatInvokeAMethodOnAQueryVariable(InvocationExpressionSyntax node)
        {
            DatabaseQuery<LINQToSQL> databaseQuery = (from n in node.DescendantNodes().OfType<IdentifierNameSyntax>()
                                                      from v in _databaseQueries.Where(dq => dq.DatabaseQueryVariable != null)
                                                      where n.Identifier.Text == v.DatabaseQueryVariable.VariableName
                                                      select v).FirstOrDefault();

            if (databaseQuery != null)
            {
                var dbAccessingMethodCall = new DatabaseAccessingMethodCallStatement<LINQToSQL>(
                    databaseQuery, node.GetCompilationInfo());

                this.DatabaseAccessingMethodCalls.Add(dbAccessingMethodCall);
                this.DatabaseAccessingMethodCallsAndSyntaxNodes.Add(dbAccessingMethodCall, node);
            }
        }
    }
}
