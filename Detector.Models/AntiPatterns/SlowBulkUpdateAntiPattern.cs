using Detector.Models.Base;

namespace Detector.Models.AntiPatterns
{
    public sealed class SlowBulkUpdateAntiPattern : AntiPatternBase
    {
        public SlowBulkUpdateAntiPattern(CodeExecutionPath codeExecutionPath, Model referenceModel)
            : base(codeExecutionPath, referenceModel)
        { }
    }
}
