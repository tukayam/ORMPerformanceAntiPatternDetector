using Detector.Extractors.Base.Helpers;
using Detector.Models.Base;
using Detector.Models.Base.CodeExecutionPaths;
using Detector.Models.ORM.ORMTools;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Immutable;

namespace Detector.Extractors.Base
{
    public abstract class CodeExecutionPathExtractor<T> where T : ORMToolType
    {
        Context<T> _context;
        public HashSet<CodeExecutionPath> CodeExecutionPaths { get; private set; }

        public CodeExecutionPathExtractor(Context<T> context)
        {
            _context = context;
            CodeExecutionPaths = new HashSet<CodeExecutionPath>();
        }

        public async Task ExtractCodeExecutionPathsAsync(Solution solution, IProgress<ExtractionProgress> progressIndicator)
        {
            progressIndicator.Report(new ExtractionProgress("Extracting code execution paths..."));

            Dictionary<MethodDeclarationSyntax, HashSet<ISymbol>> methodDecAndTheirSymbolsContainingDbAccessingMethodCalls = GetMethodDeclarationsAndTheirSymbolsForDbAccessingMethodCalls();

            int totalForProgress = GetTotalAmountForProgress(solution, methodDecAndTheirSymbolsContainingDbAccessingMethodCalls);
            int counter = 0;

            foreach (var methodDeclarationAndSymbol in methodDecAndTheirSymbolsContainingDbAccessingMethodCalls)
            {
                foreach (var project in solution.Projects)
                {
                    foreach (var document in project.Documents)
                    {
                        counter++;
                        progressIndicator.Report(new ExtractionProgress(counter * 100 / totalForProgress));

                        SyntaxNode root = await document.GetSyntaxRootAsync();
                        SemanticModel semanticModel = await document.GetSemanticModelAsync();
                        var invocations = root.DescendantNodes().OfType<InvocationExpressionSyntax>();

                        if (invocations != null && invocations.Count() > 0)
                        {
                            foreach (var symbolToMethodDeclarationOnClassOrInterface in methodDeclarationAndSymbol.Value)
                            {
                                var methodInvocations = invocations.Where(i => semanticModel.GetSymbolInfo(i).Symbol == symbolToMethodDeclarationOnClassOrInterface);
                                foreach (var invocation in methodInvocations)
                                {
                                    var methodCall = new MethodCall(methodDeclarationAndSymbol.Key, invocation.GetCompilationInfo(semanticModel));

                                    CodeExecutionPath codeExecutionPath = new CodeExecutionPath();
                                    codeExecutionPath.AddModel(methodCall);

                                    CodeExecutionPaths.Add(codeExecutionPath);
                                }
                            }
                        }
                    }
                }
            }
        }

        private Dictionary<MethodDeclarationSyntax, HashSet<ISymbol>> GetMethodDeclarationsAndTheirSymbolsForDbAccessingMethodCalls()
        {
            var methodDecAndTheirSymbolsContainingDbAccessingMethodCalls = new Dictionary<MethodDeclarationSyntax, HashSet<ISymbol>>();

            foreach (var dbAccessingCall in _context.DatabaseAccessingMethodCallStatements.Where(x => x.ParentMethodDeclarationSyntax != null))
            {
                var parentMethodDeclarationSyntax = dbAccessingCall.ParentMethodDeclarationSyntax;

                if (!methodDecAndTheirSymbolsContainingDbAccessingMethodCalls.ContainsKey(parentMethodDeclarationSyntax))
                {
                    var symbolsToMethod = new HashSet<ISymbol>();
                    SemanticModel semanticModel = dbAccessingCall.CompilationInfo.SemanticModel;

                    //Find if method declaration overwrites an interface method dec
                    ClassDeclarationSyntax classOfMethodDeclaration = parentMethodDeclarationSyntax.Ancestors().OfType<ClassDeclarationSyntax>().First();
                    var classSymbol = semanticModel.GetDeclaredSymbol(classOfMethodDeclaration);
                    ImmutableArray<INamedTypeSymbol> interfacesOfClass = classSymbol.AllInterfaces;
                    foreach (var membersInInterface in interfacesOfClass.Select(i => i.GetMembers()))
                    {
                        foreach (var member in membersInInterface)
                        {
                            if (member.Name == parentMethodDeclarationSyntax.Identifier.ToString())
                            {
                                symbolsToMethod.Add(member);
                                break;
                            }
                        }
                    }

                    var symbol = semanticModel.GetDeclaredSymbol(parentMethodDeclarationSyntax);
                    symbolsToMethod.Add(symbol);
                    methodDecAndTheirSymbolsContainingDbAccessingMethodCalls.Add(parentMethodDeclarationSyntax, symbolsToMethod);
                }
            }

            return methodDecAndTheirSymbolsContainingDbAccessingMethodCalls;
        }

        private int GetTotalAmountForProgress(Solution solution, Dictionary<MethodDeclarationSyntax, HashSet<ISymbol>> methodDecAndTheirSymbolsContainingDbAccessingMethodCalls)
        {
            int counter = 0;
            foreach (var methodDeclarationAndSymbol in methodDecAndTheirSymbolsContainingDbAccessingMethodCalls)
            {
                foreach (var project in solution.Projects)
                {
                    foreach (var document in project.Documents)
                    {
                        counter++;
                    }
                }
            }
            return counter;
        }
    }
}
