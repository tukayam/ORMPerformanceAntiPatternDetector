using Detector.Extractors.Base;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using Detector.Extractors.Base.Helpers;
using Detector.Models.Others;
using System.Data.Entity;

namespace Detector.Extractors.EF602
{
    public class DatabaseAccessingMethodCallExtractor : DatabaseAccessingMethodCallsExtractor<EntityFramework>
    {
        public ModelCollection<DatabaseAccessingMethodCallStatement<EntityFramework>> DatabaseAccessingMethodCalls { get; private set; }

        public Dictionary<DatabaseAccessingMethodCallStatement<EntityFramework>, SyntaxNode> DatabaseAccessingMethodCallsAndSyntaxNodes { get; private set; }

        private readonly ModelCollection<DatabaseEntityDeclaration<EntityFramework>> _databaseEntityDeclarations;
        private readonly ModelCollection<DatabaseQuery<EntityFramework>> _databaseQueries;
        private readonly ModelCollection<DataContextDeclaration<EntityFramework>> _dataContextDeclarations;
        private readonly SemanticModel _model;

        private Dictionary<DataContextInitializationStatement<EntityFramework>, List<DatabaseEntityVariableDeclaration<EntityFramework>>> _dataContextInitializationStatementsAndLoadedDatabaseEntityDeclarations;
        private List<VariableDeclarationSyntax> _dataContextVariables;
        private List<DataContextInitializationStatement<EntityFramework>> _dataContextInitializationStatements;
        
        public DatabaseAccessingMethodCallExtractor(Context<EntityFramework> context)
            : base(context)
        {  }
        
        public void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            //Implement chain of responsibility pattern here
            bool dbAccessingCallFound = TryExtractDatabaseAccessingMethodsThatIncludeAQueryInQuerySyntax(node);
            if (!dbAccessingCallFound)
            {
                dbAccessingCallFound = TryExtractDatabaseAccessingMethodsThatIncludeAQueryInMethodSyntax(node);
            }

            if (!dbAccessingCallFound)
            {
                TryExtractDatabaseAccessingMethodsThatInvokeAMethodOnAQueryVariable(node);
            }
            
        }

        private VariableDeclarationSyntax TryGetAssignedDataContextInstance(AssignmentExpressionSyntax node)
        {
            foreach (var variableDeclarationSyntax in node.DescendantNodes().OfType<VariableDeclarationSyntax>())
            {
                var foundVariableDecSyntax = _dataContextVariables.Where(x => x == variableDeclarationSyntax).FirstOrDefault();
                if (foundVariableDecSyntax != null)
                {
                    return foundVariableDecSyntax;
                }
            }
            return null;
        }

        private bool TryExtractDatabaseAccessingMethodsThatIncludeAQueryInQuerySyntax(InvocationExpressionSyntax node)
        {
            
            DatabaseAccessingMethodCallStatement<EntityFramework> dbAccessingMethodCall =
                (from q in node.DescendantNodes().OfType<QueryExpressionSyntax>()
                 from dq in _databaseQueries
                 where dq.IsSameQueryAs(q)
                 select new DatabaseAccessingMethodCallStatement<EntityFramework>(dq, node.GetCompilationInfo(_model))).FirstOrDefault();

            if (dbAccessingMethodCall != null)
            {
                this.DatabaseAccessingMethodCalls.Add(dbAccessingMethodCall);
                this.DatabaseAccessingMethodCallsAndSyntaxNodes.Add(dbAccessingMethodCall, node);
                return true;
            }
            return false;
        }

        private bool TryExtractDatabaseAccessingMethodsThatIncludeAQueryInMethodSyntax(InvocationExpressionSyntax node)
        {
            DatabaseAccessingMethodCallStatement<EntityFramework> dbAccessingMethodCall =
                (from q in node.DescendantNodes().OfType<IdentifierNameSyntax>()
                 where _model.GetSymbolInfo(q).Symbol.GetType().BaseType == typeof(DbContext)
                 select new DatabaseAccessingMethodCallStatement<EntityFramework>(null, node.GetCompilationInfo(_model))).FirstOrDefault();

            if (dbAccessingMethodCall != null)
            {
                this.DatabaseAccessingMethodCalls.Add(dbAccessingMethodCall);
                this.DatabaseAccessingMethodCallsAndSyntaxNodes.Add(dbAccessingMethodCall, node);
                return true;
            }
            return false;
        }

        private bool TryExtractDatabaseAccessingMethodsThatInvokeAMethodOnAQueryVariable(InvocationExpressionSyntax node)
        {
            DatabaseQuery<EntityFramework> databaseQuery = (from n in node.DescendantNodes().OfType<IdentifierNameSyntax>()
                                                      from v in _databaseQueries.Where(dq => dq.DatabaseQueryVariable != null)
                                                      where n.Identifier.Text == v.DatabaseQueryVariable.VariableName
                                                      select v).FirstOrDefault();

            if (databaseQuery != null)
            {
                var dbAccessingMethodCall = new DatabaseAccessingMethodCallStatement<EntityFramework>(
                    databaseQuery, node.GetCompilationInfo(_model));

                this.DatabaseAccessingMethodCalls.Add(dbAccessingMethodCall);
                this.DatabaseAccessingMethodCallsAndSyntaxNodes.Add(dbAccessingMethodCall, node);
                return true;
            }
            return false;
        }
    }
}
