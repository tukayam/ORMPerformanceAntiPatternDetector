using Detector.Models.Base;

namespace Detector.Models.Others
{
    public class MethodDeclaration : MethodDeclarationBase
    {
        public CompilationInfo CompilationInfo { get; private set; }

        public string MethodName { get; private set; }

        public MethodDeclaration(string methodName, CompilationInfo compilationInfo)
        {
            this.MethodName = methodName;
            this.CompilationInfo = compilationInfo;
        }
    }
}
