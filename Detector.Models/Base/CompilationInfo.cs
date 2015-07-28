using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Detector.Models.Base
{
    public class CompilationInfo
    {
        public SyntaxNode SyntaxNode { get; private set; }
        public SemanticModel SemanticModel { get; private set; }
        public string SyntaxNodeLocation
        {
            get
            {
                return SyntaxNode.GetLocation().ToString();
            }
        }      

        public CompilationInfo(SyntaxNode syntaxNode, SemanticModel semanticModel)
        {
            this.SyntaxNode = syntaxNode;
            this.SemanticModel = semanticModel;
        }
    }
}
