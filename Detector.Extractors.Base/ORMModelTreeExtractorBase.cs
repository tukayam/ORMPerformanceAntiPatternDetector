using Detector.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Detector.Extractors.Base
{
    public interface ORMModelTreeExtractor
    {
        ORMModelTree Extract(MethodDeclarationSyntax methodDeclarationSyntaxNode);
    }
}
