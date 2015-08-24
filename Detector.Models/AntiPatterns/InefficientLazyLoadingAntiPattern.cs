using Detector.Models.Base;

namespace Detector.Models.AntiPatterns
{
    public sealed class InefficientLazyLoadingAntiPattern : AntiPatternBase
    {
        public InefficientLazyLoadingAntiPattern(CodeExecutionPath codeExecutionPath, Model referenceModel)
            : base(codeExecutionPath, referenceModel)
        { }
    }
}
