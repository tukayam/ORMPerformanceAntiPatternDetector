using Detector.Models.Base;

namespace Detector.Models.AntiPatterns
{
    public sealed class SameQueryExecutionAntiPattern : AntiPatternBase
    {
        public SameQueryExecutionAntiPattern(CodeExecutionPath codeExecutionPath, Model referenceModel)
            : base(codeExecutionPath, referenceModel)
        { }
    }
}
