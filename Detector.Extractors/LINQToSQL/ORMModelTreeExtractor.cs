using Detector.Models;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Collections.Generic;
using Detector.Extractors.Helpers;
using Detector.Models.Others;

namespace Detector.Extractors
{
    /// <summary>
    /// Generates an ORMModelTree for a given MethodDeclarationSyntax (for a method)
    /// </summary>
    public class ORMModelTreeExtractor : CSharpSyntaxWalker
    {
        private ORMModelTree _ORMModelTree;
        private IEnumerable<DatabaseQuery<LINQToSQL>> _databaseQueries;

        public ORMModelTreeExtractor(IEnumerable<DatabaseQuery<LINQToSQL>> databaseQueries)
        {
            this._databaseQueries = databaseQueries;
        }

        public ORMModelTree Extract(MethodDeclarationSyntax methodDeclarationSyntaxNode)
        {
            var methodDeclaration = new MethodDeclaration(methodDeclarationSyntaxNode.Identifier.Text, methodDeclarationSyntaxNode.GetCompilationInfo());
            this._ORMModelTree = new ORMModelTree(new ORMModelNode(methodDeclaration));

            this.Visit(methodDeclarationSyntaxNode);

            return _ORMModelTree;
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            ExtractDatabaseAccessingMethodsThatIncludeAQuery(node);
            ExtractDatabaseAccessingMethodsThatInvokeAMethodOnAQueryVariable(node);
            base.VisitInvocationExpression(node);
        }

        private void ExtractDatabaseAccessingMethodsThatIncludeAQuery(InvocationExpressionSyntax node)
        {
            DatabaseAccessingMethodCallStatementOnQueryDeclaration<LINQToSQL> dbAccessingMethodCall
                = (from q in node.DescendantNodes().OfType<QueryExpressionSyntax>()
                   from dq in _databaseQueries
                   where dq.IsSameQueryAs(q)
                   select new DatabaseAccessingMethodCallStatementOnQueryDeclaration<LINQToSQL>
                   (dq, node.GetCompilationInfo())).FirstOrDefault();

            if (dbAccessingMethodCall != null)
            {
                var ORMModelNode = new ORMModelNode(dbAccessingMethodCall);
                _ORMModelTree.RootNode.ChildNodes.Add(ORMModelNode);
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
                var dbAccessingMethodCall = new DatabaseAccessingMethodCallStatementOnQueryVariable<LINQToSQL>(
                                   databaseQuery, node.GetCompilationInfo(), databaseQuery.DatabaseQueryVariable);

                if (dbAccessingMethodCall != null)
                {
                    ORMModelNode ORMModelNode = new ORMModelNode(dbAccessingMethodCall);
                    _ORMModelTree.RootNode.ChildNodes.Add(ORMModelNode);
                }
            }
        }

    }
}
