using Microsoft.CodeAnalysis;

namespace Detector.Models.Base
{
    public class CompilationInfo
    {
        public SyntaxNode SyntaxNode { get; private set; }
        public Document ContainingDocument { get; private set; }
        public Project ContainingProject { get; private set; }
        public SemanticModel SemanticModel { get; private set; }

        public CompilationInfo(SyntaxNode syntaxNode, SemanticModel semanticModel)
        {
            this.SyntaxNode = syntaxNode;
            this.SemanticModel = semanticModel;
        }
    }
}
