using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace Detector.Models.Base.CodeExecutionPaths
{
    public class MethodCall : Model
    {
        public CompilationInfo CompilationInfo { get; private set; }

        public MethodDeclarationSyntax ParentMethodDeclarationSyntax
        {
            get
            {
                var syntaxNode = this.CompilationInfo.SyntaxNode;
                return syntaxNode.AncestorsAndSelf().OfType<MethodDeclarationSyntax>().FirstOrDefault();
            }
        }

        public string MethodContainingCall
        {
            get
            {
                if (ParentMethodDeclarationSyntax != null)
                {
                    return ParentMethodDeclarationSyntax.Identifier.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public MethodDeclarationSyntax CalledMethod { get; private set; }
        public string CalledMethodName
        {
            get
            {
                return CalledMethod.Identifier.ToString();
            }
        }

        public MethodCall(MethodDeclarationSyntax calledMethod, CompilationInfo compilationInfo)
        {
            CalledMethod = calledMethod;
            CompilationInfo = compilationInfo;
        }
    }
}
