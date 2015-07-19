using System.Collections.Generic;
using System.Threading.Tasks;
using Detector.Models;
using Microsoft.CodeAnalysis;
using Detector.Models.ORM;
using Detector.Models.Base;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Detector.Extractors.Base;

namespace Detector.Extractors.LINQToSQL40
{
    public class GodClass : IGodClass
    {
        public HashSet<DatabaseEntityDeclaration<LINQToSQL>> DatabaseEntityDeclarations { get; private set; }
        public HashSet<DatabaseQuery<LINQToSQL>> DatabaseQueries { get; private set; }
        public Dictionary<DatabaseAccessingMethodCallStatement<LINQToSQL>, SyntaxNode> DatabaseAccessingMethodCalls { get; private set; }
        public Dictionary<MethodDeclarationBase, SyntaxNode> MethodDeclarations { get; private set; }
        public Dictionary<MethodCall, SyntaxNode> MethodCalls { get; private set; }

        public HashSet<HashSet<SyntaxNode>> CodeExecutionPaths { get; private set; }
        public HashSet<ORMModelTree> ORMModelTrees { get; private set; }

        public GodClass()
        {
            DatabaseEntityDeclarations = new HashSet<DatabaseEntityDeclaration<LINQToSQL>>();
            DatabaseAccessingMethodCalls = new Dictionary<DatabaseAccessingMethodCallStatement<LINQToSQL>, SyntaxNode>();
            DatabaseQueries = new HashSet<DatabaseQuery<LINQToSQL>>();
            MethodDeclarations = new Dictionary<MethodDeclarationBase, SyntaxNode>();
            MethodCalls = new Dictionary<MethodCall, SyntaxNode>();
            CodeExecutionPaths = new HashSet<HashSet<SyntaxNode>>();
            ORMModelTrees = new HashSet<ORMModelTree>();
        }

        public async Task ExtractFromRoslynSolutionAsync(Solution solution)
        {
            await ExtractEntityDeclarationsAsync(solution);
            await ExtractDatabaseQueriesAsync(solution);
            await ExtractDatabaseAccessingMethodCallsAsync(solution);

            await GenerateORMModelTreeForEachDatabaseAccessingMethodAsync(solution);
        }

        private async Task ExtractEntityDeclarationsAsync(Solution solution)
        {
            this.DatabaseEntityDeclarations = await new LINQToSQLDatabaseEntityDeclarationsExtractorOnRoslynSolution().ExtractFromSolution(solution);
        }

        private async Task ExtractDatabaseQueriesAsync(Solution solution)
        {
            foreach (var project in solution.Projects)
            {
                foreach (var documentId in project.DocumentIds)
                {
                    var document = solution.GetDocument(documentId);

                    SyntaxNode root = await Task.Run(() => document.GetSyntaxRootAsync());
                    SemanticModel model = await Task.Run(() => document.GetSemanticModelAsync());

                    var dbQueryExtractor = new LINQToSQLDatabaseQueryExtractor(model, DatabaseEntityDeclarations);
                    dbQueryExtractor.Visit(root);

                    foreach (var item in dbQueryExtractor.DatabaseQueries)
                    {
                        DatabaseQueries.Add(item);
                    }                   
                }
            }
        }

        private async Task ExtractDatabaseAccessingMethodCallsAsync(Solution solution)
        {
            foreach (var project in solution.Projects)
            {
                foreach (var documentId in project.DocumentIds)
                {
                    var document = solution.GetDocument(documentId);

                    SyntaxNode root = await Task.Run(() => document.GetSyntaxRootAsync());

                    SemanticModel model = await Task.Run(() => document.GetSemanticModelAsync());

                    var databaseAccessingMethodCalls = new LINQToSQLDatabaseAccessingMethodCallExtractor(model, DatabaseEntityDeclarations, DatabaseQueries,null,null,null);
                    databaseAccessingMethodCalls.Visit(root);

                    foreach (var item in databaseAccessingMethodCalls.DatabaseAccessingMethodCallsAndSyntaxNodes)
                    {
                        DatabaseAccessingMethodCalls.Add(item.Key, item.Value);
                    }
                }
            }
        }

        private async Task GenerateORMModelTreeForEachDatabaseAccessingMethodAsync(Solution solution)
        {
            await Task.Run(() =>
            {
                foreach (var databaseAccessingMethodCallSyntaxNode in DatabaseAccessingMethodCalls.Values)
                {
                    SyntaxNode parentMethodDeclaration = databaseAccessingMethodCallSyntaxNode.Parent;
                    while (!(parentMethodDeclaration is MethodDeclarationSyntax))
                    {
                        parentMethodDeclaration = parentMethodDeclaration.Parent;
                    }

                    var modelTreeExtractor = new RoslynORMModelTreeExtractor(this.DatabaseQueries);
                    ORMModelTree tree = modelTreeExtractor.Extract((MethodDeclarationSyntax)parentMethodDeclaration);
                    ORMModelTrees.Add(tree);
                }
            }
            );
        }
    }
}
