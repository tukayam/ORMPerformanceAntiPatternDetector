namespace Detector.Extractors.Base.Preprocessors
{
    public interface PreprocessorFactory
    {
        PreprocessorStrategy GetPreprocessor(Extractor extractor);
    }
}
