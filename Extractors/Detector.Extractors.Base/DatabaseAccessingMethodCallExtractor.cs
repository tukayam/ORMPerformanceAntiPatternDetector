using Detector.Extractors.Base.ExtensionMethods;
using Detector.Extractors.Base.Helpers;
using Detector.Models.ORM;
using Detector.Models.Others;
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

        public ModelCollection<DatabaseAccessingMethodCallStatement<T>> DatabaseAccessingMethodCalls { get; }
        public ModelCollection<DatabaseQueryVariable<T>> DatabaseQueryVariables { get; }

        public DatabaseAccessingMethodCallExtractor(Context<T> context)
            : base(context)
        {
            DatabaseAccessingMethodCalls = new ModelCollection<DatabaseAccessingMethodCallStatement<T>>();
            DatabaseQueryVariables = new ModelCollection<DatabaseQueryVariable<T>>();

            _databaseQueryVariables = new Dictionary<VariableDeclarationSyntax, SyntaxNode>();
        }

        public async Task FindDatabaseAccessingMethodCallsAsync(Solution solution, IProgress<ExtractionProgress> progress)
        {
            progress.Report(new ExtractionProgress("Started finding Database Accessing Method Calls..."));
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
                    GetQueryVariables(root, semanticModel, progress);
                    GetDatabaseQueries(root, semanticModel);
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

        private void GetQueryVariables(SyntaxNode root, SemanticModel semanticModel, IProgress<ExtractionProgress> progress)
        {            
            foreach (var node in root.DescendantNodes().OfType<VariableDeclarationSyntax>())
            {
                foreach (var queryExp in node.DescendantNodes())
                {
                    if (QueryIsDatabaseQuery(queryExp, semanticModel)
                            && !_databaseQueryVariables.ContainsKey(node))
                    {
                        _databaseQueryVariables.Add(node, queryExp);

                        var dbQueryVar = new DatabaseQueryVariable<T>(node.GetCompilationInfo(semanticModel));
                        this.DatabaseQueryVariables.Add(dbQueryVar);
                    }
                }
            }
        }

        private void GetDatabaseQueries(SyntaxNode root, SemanticModel semanticModel)
        {
            foreach (var node in root.DescendantNodes().OfType<InvocationExpressionSyntax>())
            {
                if (QueryIsDatabaseQuery(node, semanticModel))
                {
                    string queryText = node.GetText().ToString();
                    ModelCollection<DatabaseEntityDeclaration<T>> databaseEntityDeclarationsUsedInQuery = GetDatabaseEntityTypesInQuery(node, semanticModel);

                    var queryVariable = (from qv in _databaseQueryVariables
                                         where qv.Value == node
                                         select new DatabaseQueryVariable<T>(qv.Key.GetCompilationInfo(semanticModel))).FirstOrDefault();

                    var query = new DatabaseAccessingMethodCallStatement<T>(queryText, databaseEntityDeclarationsUsedInQuery, queryVariable, node.GetCompilationInfo(semanticModel));
                    DatabaseAccessingMethodCalls.Add(query);
                }
            }
        }

        private bool QueryIsDatabaseQuery(SyntaxNode query, SemanticModel model)
        {
            bool result = false;
            foreach (var qeNode in query.DescendantNodes())
            {
                result = model.IsOfType(qeNode, Context.DatabaseEntityDeclarations.Select(e => e.Name));
                if (result)
                {
                    break;
                }
            }

            return result;
        }

        private ModelCollection<DatabaseEntityDeclaration<T>> GetDatabaseEntityTypesInQuery(SyntaxNode query, SemanticModel model)
        {
            var result = new ModelCollection<DatabaseEntityDeclaration<T>>();
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
