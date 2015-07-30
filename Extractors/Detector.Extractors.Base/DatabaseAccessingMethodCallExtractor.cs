using Detector.Extractors.Base.ExtensionMethods;
using Detector.Extractors.Base.Helpers;
using Detector.Models.ORM.DatabaseAccessingMethodCalls;
using Detector.Models.ORM.DatabaseEntities;
using Detector.Models.ORM.DatabaseQueries;
using Detector.Models.ORM.ORMTools;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detector.Extractors.Base
{
    public abstract class DatabaseAccessingMethodCallExtractor<T> : Extractor<T> where T : ORMToolType
    {
        private Dictionary<VariableDeclarationSyntax, SyntaxNode> _databaseQueryVariables;

        public HashSet<DatabaseAccessingMethodCallStatement<T>> DatabaseAccessingMethodCalls { get; }
        public HashSet<DatabaseQueryVariable<T>> DatabaseQueryVariables { get; }

        public DatabaseAccessingMethodCallExtractor(Context<T> context)
            : base(context)
        {
            DatabaseAccessingMethodCalls = new HashSet<DatabaseAccessingMethodCallStatement<T>>();
            DatabaseQueryVariables = new HashSet<DatabaseQueryVariable<T>>();

            _databaseQueryVariables = new Dictionary<VariableDeclarationSyntax, SyntaxNode>();
        }

        public async Task FindDatabaseAccessingMethodCallsAsync(Solution solution, IProgress<ExtractionProgress> progress)
        {
            progress.Report(new ExtractionProgress("Finding Database Accessing Method Calls..."));
            int totalAmountOfDocs = GetTotalAmountOfDocuments(solution);

            int counter = 0;
            foreach (var project in solution.Projects)
            {
                foreach (var document in project.Documents)
                {
                    counter++;
                    progress.Report(GetExtractionProgress(totalAmountOfDocs, counter));

                    SyntaxNode root = await document.GetSyntaxRootAsync();
                    SemanticModel semanticModel = await document.GetSemanticModelAsync();

                    GetDatabaseQueryVariablesInDocument(root, semanticModel);
                    GetDatabaseAccessingSelectStatementsInQuerySyntax(root, semanticModel);
                    GetDatabaseAccessingCallsInMethodSyntax(root, semanticModel);
                }
            }

            Context.DatabaseAccessingMethodCallStatements = DatabaseAccessingMethodCalls;
            Context.DatabaseQueryVariables = DatabaseQueryVariables;
        }

        private int _totalAmountOfDocuments;
        private int GetTotalAmountOfDocuments(Solution solution)
        {
            if (_totalAmountOfDocuments == 0)
            {
                int counter = 0;
                foreach (var project in solution.Projects)
                {
                    foreach (var document in project.Documents)
                    {
                        counter++;
                    }
                }
                _totalAmountOfDocuments = counter;
            }
            return _totalAmountOfDocuments;
        }

        private ExtractionProgress GetExtractionProgress(int total, int counter)
        {
            return new ExtractionProgress(counter * 100 / total);
        }

        private void GetDatabaseQueryVariablesInDocument(SyntaxNode root, SemanticModel semanticModel)
        {
            foreach (var node in root.DescendantNodes().OfType<VariableDeclarationSyntax>())
            {
                var varDeclarator = node.DescendantNodes().OfType<VariableDeclaratorSyntax>().First();

                if (SyntaxNodeIsDatabaseQuery(varDeclarator, semanticModel)
                        && !_databaseQueryVariables.ContainsKey(node))
                {
                    var dbQueryVar = new DatabaseQueryVariable<T>(node.Variables[0].Identifier.ToString(), node.GetCompilationInfo(semanticModel));
                    this.DatabaseQueryVariables.Add(dbQueryVar);
                }
            }
        }

        private void GetDatabaseAccessingSelectStatementsInQuerySyntax(SyntaxNode rootOfDocument, SemanticModel semanticModel)
        {
            foreach (var invocationExpSyntax in rootOfDocument.DescendantNodes().OfType<InvocationExpressionSyntax>())
            {
                //If Query is defined within the same line
                var queryExpSyntax = invocationExpSyntax.DescendantNodes().OfType<QueryExpressionSyntax>().FirstOrDefault();
                if (queryExpSyntax != null)
                {
                    if (SyntaxNodeIsDatabaseQuery(queryExpSyntax, semanticModel))
                    {
                        AddDatabaseAccessingCall(invocationExpSyntax, semanticModel);
                    }
                }
                //else if invocation is done on an already defined query variable 
                else
                {
                    MemberAccessExpressionSyntax memberAccessExpSyntax = invocationExpSyntax.DescendantNodes().OfType<MemberAccessExpressionSyntax>().FirstOrDefault();

                    if (memberAccessExpSyntax != null)
                    {
                        var queryVariable = (from identifier in memberAccessExpSyntax.DescendantNodes().OfType<IdentifierNameSyntax>()
                                             from queryVar in DatabaseQueryVariables
                                             where identifier.Identifier.ToString() == queryVar.VariableName
                                             select queryVar).FirstOrDefault();

                        if (queryVariable != null)
                        {
                            AddDatabaseAccessingCall(invocationExpSyntax, semanticModel, queryVariable);
                        }
                    }
                }
            }
        }

        private void GetDatabaseAccessingCallsInMethodSyntax(SyntaxNode rootOfDocument, SemanticModel semanticModel)
        {
            List<SyntaxNode> nodesToCheck = new List<SyntaxNode>();
            nodesToCheck.AddRange(rootOfDocument.DescendantNodes().OfType<InvocationExpressionSyntax>());

            foreach (var node in nodesToCheck)
            {
                if (SyntaxNodeIsDatabaseQuery(node, semanticModel))
                {
                    AddDatabaseAccessingCall(node, semanticModel);
                }
            }
        }

        private void AddDatabaseAccessingCall(SyntaxNode node, SemanticModel semanticModel)
        {
            AddDatabaseAccessingCall(node, semanticModel, null);
        }

        private void AddDatabaseAccessingCall(SyntaxNode node, SemanticModel semanticModel, DatabaseQueryVariable<T> queryVariableUsedInCall)
        {
            string queryText = node.GetText().ToString();
            HashSet<DatabaseEntityDeclaration<T>> databaseEntityDeclarationsUsedInQuery = GetDatabaseEntityTypesInQuery(node, semanticModel);
            var compilationInfo = node.GetCompilationInfo(semanticModel);


            if (!AncestorNodeForSameLineIsAlreadyFound(node))
            {
                RemoveAnyDescendantNodeAlreadyFound(node);
                var result = new DatabaseAccessingMethodCallStatement<T>(queryText, databaseEntityDeclarationsUsedInQuery, compilationInfo);

                if (queryVariableUsedInCall != null)
                {
                    result.SetDatabaseQueryVariable(queryVariableUsedInCall);
                }

                DatabaseAccessingMethodCalls.Add(result);
            }
            
        }

        private bool AncestorNodeForSameLineIsAlreadyFound(SyntaxNode node)
        {
            DatabaseAccessingMethodCallStatement<T> existingDbAccessingCall = (from d in DatabaseAccessingMethodCalls
                                                                               from x in d.CompilationInfo.SyntaxNode.DescendantNodes()
                                                                               where x.Span.ToString() == node.Span.ToString()
                                                                               select d).FirstOrDefault();

            return existingDbAccessingCall != null;
        }

        private void RemoveAnyDescendantNodeAlreadyFound(SyntaxNode node)
        {
            DatabaseAccessingMethodCallStatement<T> existingDbAccessingCall;
            foreach (var item in node.DescendantNodes())
            {
                existingDbAccessingCall = DatabaseAccessingMethodCalls.FirstOrDefault(x => x.CompilationInfo.SyntaxNode.Span.ToString() == node.Span.ToString());
                if (existingDbAccessingCall != null)
                {
                    DatabaseAccessingMethodCalls.Remove(existingDbAccessingCall);
                }
            }
        }

        private bool SyntaxNodeIsDatabaseQuery(SyntaxNode node, SemanticModel model)
        {
            bool result = false;
            foreach (var qeNode in node.DescendantNodes())
            {
                result = model.IsOfType(qeNode, Context.DatabaseEntityDeclarations.Select(e => e.Name));
                if (result)
                {
                    break;
                }
            }

            return result;
        }

        private HashSet<DatabaseEntityDeclaration<T>> GetDatabaseEntityTypesInQuery(SyntaxNode query, SemanticModel model)
        {
            var result = new HashSet<DatabaseEntityDeclaration<T>>();
            foreach (var qeNode in query.DescendantNodes())
            {
                ITypeSymbol typeOfNode = model.GetTypeInfo(qeNode).Type;
                if (typeOfNode != null)
                {
                    var entityDeclarationInQuery = Context.DatabaseEntityDeclarations.Where(e => typeOfNode.ToString().Contains(e.Name)).FirstOrDefault();
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
