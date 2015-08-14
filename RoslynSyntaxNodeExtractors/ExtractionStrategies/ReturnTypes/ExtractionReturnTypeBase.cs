using Microsoft.CodeAnalysis;

namespace Detector.Extractors.Base.ExtractionStrategies.ReturnTypes
{
    public abstract class ExtractionReturnTypeBase
    {
        public SemanticModel SemanticModel { get; private set; }

        public ExtractionReturnTypeBase(SemanticModel semanticModel)
        {
            SemanticModel = semanticModel;
        }
    }
}
