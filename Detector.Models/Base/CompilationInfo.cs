namespace Detector.Models.Base
{
    public class CompilationInfo
    {
        public string FilePath { get; private set; }
        public string FileName { get; private set; }
        
        public int LineNumberStart { get; private set; }

        public MethodDeclarationBase ParentMethodDeclaration { get; private set; }

        public CompilationInfo(string filePath, string fileName, int lineNumberStart)
        {
            this.FilePath = filePath;
            this.FileName = fileName;
            this.LineNumberStart = lineNumberStart;
        }

        public void SetParentMethodDeclaration(MethodDeclarationBase parentMethodDeclaration)
        {
            this.ParentMethodDeclaration = parentMethodDeclaration;
        }
    }
}
