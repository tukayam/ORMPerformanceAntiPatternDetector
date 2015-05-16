namespace Detector.Models.Compilation
{
    public class CompilationInfo
    {
        public string FilePath { get; private set; }
        public string FileName { get; private set; }
        
        public MethodDeclaration ParentMethodDeclaration { get; private set; }

        public CompilationInfo(string filePath, string fileName)
        {
            this.FilePath = filePath;
            this.FileName = fileName;
        }

        public void SetParentMethodDeclaration(MethodDeclaration parentMethodDeclaration)
        {
            this.ParentMethodDeclaration = parentMethodDeclaration;
        }
    }
}
