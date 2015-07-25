using Detector.Models.ORM;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using Detector.Extractors.Base.Helpers;
using System.Threading.Tasks;
using Detector.Models.Others;

namespace Detector.Extractors.Base
{
    public abstract class DatabaseAccessingMethodCallExtractor<T> : Extractor<T> where T : ORMToolType
    {
        public ModelCollection<DatabaseAccessingMethodCallStatement<T>> DatabaseAccessingMethodCalls { get; }

        public async Task FindDatabaseAccessingMethodCallStatementsAsync(Solution solution)
        {
            var result = new Dictionary<ClassDeclarationSyntax, SemanticModel>();
            foreach (var project in solution.Projects)
            {
                foreach (var document in project.Documents)
                {
                    SyntaxNode root = await document.GetSyntaxRootAsync();
                    SemanticModel semanticModel = await document.GetSemanticModelAsync();

                    foreach (InvocationExpressionSyntax node in root.DescendantNodes().OfType<InvocationExpressionSyntax>())
                    {
                        bool dbAccessingCallFound = TryExtractDatabaseAccessingMethodsThatIncludeAQueryInQuerySyntax(node, semanticModel);
                        if (!dbAccessingCallFound)
                        {
                            dbAccessingCallFound = TryExtractDatabaseAccessingMethodsThatIncludeAQueryInMethodSyntax(node, semanticModel);
                        }

                        if (!dbAccessingCallFound)
                        {
                            TryExtractDatabaseAccessingMethodsThatInvokeAMethodOnAQueryVariable(node, semanticModel);
                        }
                    }
                }
            }
            Context.DatabaseAccessingMethodCallStatements = DatabaseAccessingMethodCalls;
        }
        
        public DatabaseAccessingMethodCallExtractor(Context<T> context)
            : base(context)
        {
            DatabaseAccessingMethodCalls = new ModelCollection<DatabaseAccessingMethodCallStatement<T>>();
        }

        private bool TryExtractDatabaseAccessingMethodsThatIncludeAQueryInQuerySyntax(InvocationExpressionSyntax node, SemanticModel model)
        {

            DatabaseAccessingMethodCallStatement<T> dbAccessingMethodCall =
                (from q in node.DescendantNodes().OfType<QueryExpressionSyntax>()
                 from dq in Context.DatabaseQueries
                 where dq.IsSameQueryAs(q)
                 select new DatabaseAccessingMethodCallStatement<T>(dq, node.GetCompilationInfo(model))).FirstOrDefault();

            if (dbAccessingMethodCall != null)
            {
                this.DatabaseAccessingMethodCalls.Add(dbAccessingMethodCall);
                return true;
            }
            return false;
        }

        private bool TryExtractDatabaseAccessingMethodsThatIncludeAQueryInMethodSyntax(InvocationExpressionSyntax node, SemanticModel model)
        {
            //DatabaseAccessingMethodCallStatement<T> dbAccessingMethodCall =
            //    (from q in node.DescendantNodes().OfType<IdentifierNameSyntax>()
            //     where model.GetSymbolInfo(q).Symbol.GetType().BaseType == typeof(DbContext)
            //     select new DatabaseAccessingMethodCallStatement<T>(null, node.GetCompilationInfo(model))).FirstOrDefault();

            //if (dbAccessingMethodCall != null)
            //{
            //    this.DatabaseAccessingMethodCalls.Add(dbAccessingMethodCall);
            //    return true;
            //}
            return false;
        }

        private bool TryExtractDatabaseAccessingMethodsThatInvokeAMethodOnAQueryVariable(InvocationExpressionSyntax node, SemanticModel model)
        {
            DatabaseQuery<T> databaseQuery = (from n in node.DescendantNodes().OfType<IdentifierNameSyntax>()
                                                            from v in Context.DatabaseQueries.Where(dq => Context.DatabaseQueryVariables.Contains(dq.DatabaseQueryVariable))
                                                            select v).FirstOrDefault();

            if (databaseQuery != null)
            {
                var dbAccessingMethodCall = new DatabaseAccessingMethodCallStatement<T>(
                    databaseQuery, node.GetCompilationInfo(model));

                this.DatabaseAccessingMethodCalls.Add(dbAccessingMethodCall);
                return true;
            }
            return false;
        }

        //private VariableDeclarationSyntax TryGetAssignedDataContextInstance(AssignmentExpressionSyntax node)
        //{
        //    foreach (var variableDeclarationSyntax in node.DescendantNodes().OfType<VariableDeclarationSyntax>())
        //    {
        //        var foundVariableDecSyntax = _dataContextVariables.Where(x => x == variableDeclarationSyntax).FirstOrDefault();
        //        if (foundVariableDecSyntax != null)
        //        {
        //            return foundVariableDecSyntax;
        //        }
        //    }
        //    return null;
        //}
    }
}
