namespace Detector.Models.Base
{
    public class CompilationInfo
    {
        public int SpanStart { get; private set; }

        public MethodDeclarationBase ParentMethodDeclaration { get; private set; }

        public CompilationInfo(int spanStart)
        {
            this.SpanStart = spanStart;
        }

        public void SetParentMethodDeclaration(MethodDeclarationBase parentMethodDeclaration)
        {
            this.ParentMethodDeclaration = parentMethodDeclaration;
        }
    }
}
