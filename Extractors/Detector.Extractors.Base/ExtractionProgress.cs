namespace Detector.Extractors.Base
{
    public class ExtractionProgress
    {
        public string ExtractionType { get; set; }
        public int PercentageOfWorkDone { get; set; }

        public ExtractionProgress(string extractionType)
        {
            this.ExtractionType = extractionType;
        }

        public ExtractionProgress(int percentage)
        {
            PercentageOfWorkDone = percentage;
        }
    }
}
