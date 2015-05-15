using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Detector.Main.Tests.Helper
{
    public static class RoslynSyntaxTreeParser
    {
        public static SyntaxNode GetRootSyntaxNodeForText(string text)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(text);
            return tree.GetRoot();
        }
    }
}
