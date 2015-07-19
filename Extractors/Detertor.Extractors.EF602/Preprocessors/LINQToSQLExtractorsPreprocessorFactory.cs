using Detector.Extractors.Base.Preprocessors;
using Detector.Extractors.Base;

namespace Detertor.Extractors.LINQToSQL40.Preprocessors
{
    class LINQToSQLExtractorsPreprocessorFactory : PreprocessorFactory
    {
        public PreprocessorStrategy GetPreprocessor(Extractor extractor)
        {
            //return extractor.GetPreprocessor();
            return null;
        }
    }
}
