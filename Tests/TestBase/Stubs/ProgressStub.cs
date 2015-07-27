using Detector.Extractors.Base;
using System;

namespace TestBase.Stubs
{
    public class ProgressStub : IProgress<ExtractionProgress>
    {
        public void Report(ExtractionProgress value)
        {
            
        }
    }
}
