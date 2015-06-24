using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Detector.Extractors.Tests.Helpers.RoslynSolutionGenerators
{
    public static class RoslynSyntaxTreeParser
    {
        public static SyntaxNode GetRootSyntaxNodeForText(string text)
        {
            SyntaxTree tree = GetSyntaxTreeForText(text);
            return tree.GetRoot();
        }

        public static SyntaxTree GetSyntaxTreeForText(string text)
        {
            return CSharpSyntaxTree.ParseText(text);
        }
    }
}
