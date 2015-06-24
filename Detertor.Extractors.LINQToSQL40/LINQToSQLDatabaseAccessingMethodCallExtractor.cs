﻿using Detector.Extractors.Base;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using Detector.Extractors.Base.Helpers;

namespace Detector.Extractors.LINQToSQL40
{
    public class LINQToSQLDatabaseAccessingMethodCallExtractor : CSharpSyntaxWalker, DatabaseAccessingMethodCallExtractor<LINQToSQL>
    {
        public HashSet<DatabaseAccessingMethodCallStatement<LINQToSQL>> DatabaseAccessingMethodCalls { get; private set; }

        public Dictionary<DatabaseAccessingMethodCallStatement<LINQToSQL>, SyntaxNode> DatabaseAccessingMethodCallsAndSyntaxNodes { get; private set; }

        private readonly HashSet<DatabaseEntityDeclaration<LINQToSQL>> _databaseEntityDeclarations;
        private readonly HashSet<DatabaseQuery<LINQToSQL>> _databaseQueries;
        private readonly HashSet<DataContextDeclaration<LINQToSQL>> _dataContextDeclarations;
        private readonly SemanticModel _model;

        private Dictionary<DataContextInitializationStatement<LINQToSQL>, List<DatabaseEntityVariableDeclaration<LINQToSQL>>> _dataContextInitializationStatementsAndLoadedDatabaseEntityDeclarations;
        private List<VariableDeclarationSyntax> _dataContextVariables;
        private List<DataContextInitializationStatement<LINQToSQL>> _dataContextInitializationStatements;
        private List<VariableDeclarationSyntax> _dataLoadOptionsVariables;

        public LINQToSQLDatabaseAccessingMethodCallExtractor(SemanticModel model
             , HashSet<DatabaseEntityDeclaration<LINQToSQL>> databaseEntityDeclarations
             , HashSet<DatabaseQuery<LINQToSQL>> databaseQueries
             , List<VariableDeclarationSyntax> dataLoadOptionsVariables
             , List<DataContextInitializationStatement<LINQToSQL>> dataContextInitializationStatements
             , HashSet<DataContextDeclaration<LINQToSQL>> dataContextDeclarations
           )
            : base()
        {
            this._model = model;
            this._databaseEntityDeclarations = databaseEntityDeclarations;
            this._dataContextDeclarations = dataContextDeclarations;
            this._databaseQueries = databaseQueries;
            this._dataLoadOptionsVariables = dataLoadOptionsVariables;
            this._dataContextInitializationStatements = dataContextInitializationStatements;

            this.DatabaseAccessingMethodCalls = new HashSet<DatabaseAccessingMethodCallStatement<LINQToSQL>>();
            this.DatabaseAccessingMethodCallsAndSyntaxNodes = new Dictionary<DatabaseAccessingMethodCallStatement<LINQToSQL>, SyntaxNode>();

            this._dataContextInitializationStatementsAndLoadedDatabaseEntityDeclarations = new Dictionary<DataContextInitializationStatement<LINQToSQL>, List<DatabaseEntityVariableDeclaration<LINQToSQL>>>();
        }

        private void MatchLoadedDatabaseEntityDeclarationsForDataContextInitializationStatement(List<VariableDeclarationSyntax> dataLoadOptionsVariables, List<DataContextInitializationStatement<LINQToSQL>> dataContextInitializationStatements)
        {
            foreach (VariableDeclarationSyntax dataLoadOptionsVariableSyntax in dataLoadOptionsVariables)
            {

            }
        }

        //public override void VisitAssignmentExpression(AssignmentExpressionSyntax node)
        //{
        //    VariableDeclarationSyntax assignedDataContextVariable = TryGetAssignedDataContextInstance(node);
        //    if (assignedDataContextVariable != null)
        //    {
        //        assignedDataContextVariable.DescendantNodes().Where(x=>x.Get)
        //    }
        //    base.VisitAssignmentExpression(node);
        //}


        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            ExtractDatabaseAccessingMethodsThatIncludeAQueryInQuerySyntax(node);
            ExtractDatabaseAccessingMethodsThatIncludeAQueryInMethodSyntax(node);
            ExtractDatabaseAccessingMethodsThatInvokeAMethodOnAQueryVariable(node);

            base.VisitInvocationExpression(node);
        }

        private VariableDeclarationSyntax TryGetInvokedDataLoadOptionsVariable(InvocationExpressionSyntax node)
        {
            foreach (var variableDeclarationSyntax in node.DescendantNodes().OfType<VariableDeclarationSyntax>())
            {
                var foundVariableDecSyntax = _dataLoadOptionsVariables.Where(x => x == variableDeclarationSyntax).FirstOrDefault();
                if (foundVariableDecSyntax != null)
                {
                    return foundVariableDecSyntax;
                }
            }
            return null;
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

        private void ExtractDatabaseAccessingMethodsThatIncludeAQueryInQuerySyntax(InvocationExpressionSyntax node)
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

        private void ExtractDatabaseAccessingMethodsThatIncludeAQueryInMethodSyntax(InvocationExpressionSyntax node)
        {
            DatabaseAccessingMethodCallStatement<LINQToSQL> dbAccessingMethodCall =
                (from q in node.DescendantNodes().OfType<IdentifierNameSyntax>()
                 where _model.GetSymbolInfo(q).Symbol.GetType().BaseType == typeof(System.Data.Linq.DataContext)
                 select new DatabaseAccessingMethodCallStatement<LINQToSQL>(null, node.GetCompilationInfo())).FirstOrDefault();

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