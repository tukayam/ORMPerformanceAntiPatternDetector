using Detector.Models.Base;
using Detector.Models.Others;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Detector.Extractors.Helpers
{
    public static class SyntaxNodeExtensions
    {
        public static CompilationInfo GetCompilationInfo(this SyntaxNode node)
        {
            var compilationInfo = new CompilationInfo(node.SpanStart);

            MethodDeclaration parentMethodDeclaration = null;
            SyntaxNode parentNode = node.Parent;
            do
            {
                if (parentNode is MethodDeclarationSyntax)
                {
                    parentMethodDeclaration = new MethodDeclaration((parentNode as MethodDeclarationSyntax).Identifier.ToString(), parentNode.GetCompilationInfo());
                }
                else
                {
                    parentNode = parentNode.Parent;
                }
            } while (parentMethodDeclaration == null && parentNode != null);

            compilationInfo.SetParentMethodDeclaration(parentMethodDeclaration);

            return compilationInfo;
        }
    }
}
