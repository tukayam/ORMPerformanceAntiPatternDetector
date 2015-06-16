using System.Collections.Generic;
using System.Threading.Tasks;
using Detector.Models;
using Microsoft.CodeAnalysis;
using Detector.Models.ORM;
using Detector.Extractors.DatabaseEntities;
using Detector.Models.Base;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace Detector.Extractors
{
    public class GodClass
    {
        public List<DatabaseEntityDeclaration<LINQToSQL>> DatabaseEntityDeclarations { get; private set; }
        public List<DatabaseQuery<LINQToSQL>> DatabaseQueries { get; private set; }
        public Dictionary<DatabaseAccessingMethodCallStatement<LINQToSQL>, SyntaxNode> DatabaseAccessingMethodCalls { get; private set; }
        public Dictionary<MethodDeclarationBase, SyntaxNode> MethodDeclarations { get; private set; }
        public Dictionary<MethodCall, SyntaxNode> MethodCalls { get; private set; }

        public List<List<SyntaxNode>> CodeExecutionPaths { get; private set; }
        public List<ORMModelTree> ORMModelTrees { get; private set; }

        public GodClass()
        {
            DatabaseEntityDeclarations = new List<DatabaseEntityDeclaration<LINQToSQL>>();
            DatabaseAccessingMethodCalls = new Dictionary<DatabaseAccessingMethodCallStatement<LINQToSQL>, SyntaxNode>();
            DatabaseQueries = new List<DatabaseQuery<LINQToSQL>>();
            MethodDeclarations = new Dictionary<MethodDeclarationBase, SyntaxNode>();
            MethodCalls = new Dictionary<MethodCall, SyntaxNode>();
            CodeExecutionPaths = new List<List<SyntaxNode>>();
            ORMModelTrees = new List<ORMModelTree>();
        }

        public async void ExtractFromRoslynSolution(Solution solution)
        {
            ExtractEntityDeclarations(solution);
            ExtractDatabaseQueries(solution);
            ExtractDatabaseAccessingMethodCalls(solution);

            GenerateORMModelTreeForEachDatabaseAccessingMethod(solution);
        }

        private async void ExtractEntityDeclarations(Solution solution)
        {
            foreach (var project in solution.Projects)
            {
                foreach (var documentId in project.DocumentIds)
                {
                    var document = solution.GetDocument(documentId);

                    SyntaxNode root = await Task.Run(() => document.GetSyntaxRootAsync());

                    var dbEntityDeclarationExtractor = new LINQToSQLDatabaseEntityDeclarationExtractor();
                    dbEntityDeclarationExtractor.Visit(root);
                    this.DatabaseEntityDeclarations.AddRange(dbEntityDeclarationExtractor.DatabaseEntityDeclarations);
                }
            }
        }

        private async void ExtractDatabaseQueries(Solution solution)
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
                    DatabaseQueries.AddRange(dbQueryExtractor.DatabaseQueries);
                }
            }
        }

        private async void ExtractDatabaseAccessingMethodCalls(Solution solution)
        {
            foreach (var project in solution.Projects)
            {
                foreach (var documentId in project.DocumentIds)
                {
                    var document = solution.GetDocument(documentId);

                    SyntaxNode root = await Task.Run(() => document.GetSyntaxRootAsync());

                    SemanticModel model = await Task.Run(() => document.GetSemanticModelAsync());

                    var databaseAccessingMethodCalls = new LINQToSQLDatabaseAccessingMethodCallExtractor(model, DatabaseEntityDeclarations, DatabaseQueries);
                    databaseAccessingMethodCalls.Visit(root);

                    foreach (var item in databaseAccessingMethodCalls.DatabaseAccessingMethodCallsAndSyntaxNodes)
                    {
                        DatabaseAccessingMethodCalls.Add(item.Key, item.Value);
                    }
                }
            }
        }

        private async void ExtractAllInternalMethodCalls(Solution solution)
        {
            MethodDeclarations = new Dictionary<MethodDeclarationBase, SyntaxNode>();
        }

        private async void GenerateORMModelTreeForEachDatabaseAccessingMethod(Solution solution)
        {
            //foreach DbAccessingMethodCall, get SyntaxNode, find parent methodDeclaration
            foreach (var databaseAccessingMethodCallSyntaxNode in DatabaseAccessingMethodCalls.Values)
            {
                SyntaxNode parentMethodDeclaration = null;
                do
                {
                    parentMethodDeclaration = databaseAccessingMethodCallSyntaxNode.Parent;
                } while (!(parentMethodDeclaration is MethodDeclarationSyntax));

                //check if there are any direct calls to methodDeclaration found (in extracted all method calls)

                ExtractCodeExecutionTrees(parentMethodDeclaration);

                //check if the class of methodDeclaration found has interfaces and if any interface contain methoddeclaration, then check if any methodcalls exist to that interface methoddeclaration
                SyntaxNode methodDeclarationOnInterface = GetMethodDeclarationOnInterface(parentMethodDeclaration);
                ExtractCodeExecutionTrees(methodDeclarationOnInterface);

                //check if the class of methodDeclaration found has abstract class derivation and if any interface contain methoddeclaration, then check if any methodcalls exist to that interface methoddeclaration
                SyntaxNode methodDeclarationOnAbstractClass = GetMethodDeclarationOnAbstractClass(parentMethodDeclaration);
                ExtractCodeExecutionTrees(methodDeclarationOnAbstractClass);
            }

            ConvertCodeExecutionPathsIntoORMModelTree();

            ORMModelTrees = null;
        }

        private void ConvertCodeExecutionPathsIntoORMModelTree()
        {
            throw new NotImplementedException();
        }

        private void ExtractCodeExecutionTrees(SyntaxNode methodContainingDatabaseAccessingCall)
        {
            SyntaxNode calledMethod = methodContainingDatabaseAccessingCall;
            List<SyntaxNode> callingMethods;
            do
            {
                callingMethods = (from md in MethodCalls
                                  where md.Key.CalledMethod == calledMethod
                                  select md.Value).ToList();
                foreach (var callingMethod in callingMethods)
                {
                    var codeExecutionPath = new List<SyntaxNode>();
                    codeExecutionPath.Add(callingMethod);
                    CodeExecutionPaths.Add(codeExecutionPath);

                    calledMethod = callingMethod;
                    ExtractCodeExecutionPaths(calledMethod, codeExecutionPath);
                }
            } while (callingMethods != null);
        }

        private void ExtractCodeExecutionPaths(SyntaxNode calledMethod, List<SyntaxNode> codeExecutionPath)
        {
            List<SyntaxNode> callingMethods = (from md in MethodCalls
                                               where md.Key.CalledMethod == calledMethod
                                               select md.Value).ToList();

            foreach (var callingMethod in callingMethods)
            {
                var newQueue = new List<SyntaxNode>();
                newQueue.AddRange(codeExecutionPath);

                CodeExecutionPaths.Remove(codeExecutionPath);
                CodeExecutionPaths.Add(newQueue);

                ExtractCodeExecutionPaths(callingMethod, newQueue);
            }
        }

        private SyntaxNode GetMethodDeclarationOnInterface(SyntaxNode methodDeclarationOnClass)
        {
            SyntaxNode methodDeclarationOnInterface = null;

            SyntaxNode classDeclaration;
            do
            {
                classDeclaration = methodDeclarationOnClass.Parent;
            } while (!(classDeclaration is ClassDeclarationSyntax));

            return null;
        }

        private SyntaxNode GetMethodDeclarationOnAbstractClass(SyntaxNode methodDeclarationOnClass)
        {
            SyntaxNode methodDeclarationOnInterface = null;

            SyntaxNode classDeclaration;
            do
            {
                classDeclaration = methodDeclarationOnClass.Parent;
            } while (!(classDeclaration is ClassDeclarationSyntax));

            return null;
        }
    }
}
