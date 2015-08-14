using Microsoft.CodeAnalysis;

namespace Detector.Extractors.Base.ExtractionStrategies
{
    public class SolutionParameter : ParameterBase<Solution>
    {
        public SolutionParameter(string name, Solution value)
            : base(name, value)
        { }
    }
}
