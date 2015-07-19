using System.Collections.Generic;
using Detector.Models;
using Microsoft.CodeAnalysis;
using Detector.Models.ORM;
using Detector.Models.Base;
using System.Threading.Tasks;

namespace Detector.Extractors.Base
{
    public interface IGodClass
    {
        List<DatabaseEntityDeclaration<LINQToSQL>> DatabaseEntityDeclarations { get; }
        List<DatabaseQuery<LINQToSQL>> DatabaseQueries { get; }
        Dictionary<DatabaseAccessingMethodCallStatement<LINQToSQL>, SyntaxNode> DatabaseAccessingMethodCalls { get; }
        Dictionary<MethodDeclarationBase, SyntaxNode> MethodDeclarations { get; }
        Dictionary<MethodCall, SyntaxNode> MethodCalls { get; }
        List<List<SyntaxNode>> CodeExecutionPaths { get; }
        List<ORMModelTree> ORMModelTrees { get; }

        Task ExtractFromRoslynSolutionAsync(Solution solution);
    }
}
