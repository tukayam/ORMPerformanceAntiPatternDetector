using Detector.Models.Base;

namespace Detector.Models.AntiPatterns
{
    public sealed class UnnecessaryObjectTrackingAntiPattern : AntiPatternBase
    {
        public UnnecessaryObjectTrackingAntiPattern(CodeExecutionPath codeExecutionPath, Model referenceModel)
            : base(codeExecutionPath, referenceModel)
        { }
    }
}
