namespace Detector.Models.Base
{
    public class MethodCall : ModelBase
    {
        public CompilationInfo CompilationInfo { get; private set; }
        public MethodDeclarationBase CalledMethod { get; private set; }

        public MethodCall(MethodDeclarationBase calledMethod, CompilationInfo compilationInfo)
        {
            this.CalledMethod = calledMethod;
            this.CompilationInfo = compilationInfo;
        }
    }
}
