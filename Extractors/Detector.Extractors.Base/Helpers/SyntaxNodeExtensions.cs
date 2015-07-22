using Detector.Models.Base;
using Microsoft.CodeAnalysis;

namespace Detector.Extractors.Base.Helpers
{
    public static class SyntaxNodeExtensions
    {
        public static CompilationInfo GetCompilationInfo(this SyntaxNode node, SemanticModel model)
        {
            var compilationInfo = new CompilationInfo(node, model);

            return compilationInfo;
        }
    }
}
