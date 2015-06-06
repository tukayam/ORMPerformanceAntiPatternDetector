namespace Detector.Models.Others
{
    public class Document
    {
        public string FilePath { get; private set; }
        public string FileName { get; private set; }

        public Document(string filePath, string fileName)
        {
            this.FilePath = filePath;
            this.FileName = fileName;
        }
    }
}
