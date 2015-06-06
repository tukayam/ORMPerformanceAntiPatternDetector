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
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            ExtractDatabaseAccessingMethodsThatIncludeAQuery(node);
            ExtractDatabaseAccessingMethodsThatInvokeAMethodOnAQueryVariable(node);
            base.VisitInvocationExpression(node);
        }

        private void ExtractDatabaseAccessingMethodsThatIncludeAQuery(InvocationExpressionSyntax node)
        {
            var dbAccessingMethodCalls = (from q in node.DescendantNodes().OfType<QueryExpressionSyntax>()
                                          from dq in _databaseQueries
                                          where dq.IsSameQueryAs(q)
                                          select new DatabaseAccessingMethodCallStatementOnQueryDeclaration<LINQToSQL>(dq, node.GetCompilationInfo()));

            this.DatabaseAccessingMethodCalls.AddRange(dbAccessingMethodCalls.ToList());
        }

        private void ExtractDatabaseAccessingMethodsThatInvokeAMethodOnAQueryVariable(InvocationExpressionSyntax node)
        {
            DatabaseQuery<LINQToSQL> databaseQuery = (from n in node.DescendantNodes().OfType<IdentifierNameSyntax>()
                                                      from v in _databaseQueries.Where(dq => dq.DatabaseQueryVariable != null)
                                                      where n.Identifier.Text == v.DatabaseQueryVariable.VariableName
                                                      select v).FirstOrDefault();

            if (databaseQuery != null)
            {
                this.DatabaseAccessingMethodCalls.Add(new DatabaseAccessingMethodCallStatementOnQueryVariable<LINQToSQL>(
                    databaseQuery, node.GetCompilationInfo(), databaseQuery.DatabaseQueryVariable));
            }
        }
    }
}
