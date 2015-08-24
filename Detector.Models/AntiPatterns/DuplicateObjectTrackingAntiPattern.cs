using Detector.Models.Base;

namespace Detector.Models.AntiPatterns
{
    public sealed class DuplicateObjectTrackingAntiPattern : AntiPatternBase
    {
        public DuplicateObjectTrackingAntiPattern(CodeExecutionPath codeExecutionPath, Model referenceModel)
            : base(codeExecutionPath, referenceModel)
        { }
    }
}
