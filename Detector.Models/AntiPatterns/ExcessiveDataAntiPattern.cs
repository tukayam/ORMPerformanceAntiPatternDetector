using Detector.Models.Base;

namespace Detector.Models.AntiPatterns
{
    public sealed class ExcessiveDataAntiPattern : AntiPatternBase
    {
        public ExcessiveDataAntiPattern(CodeExecutionPath codeExecutionPath, Model referenceModel)
            : base(codeExecutionPath, referenceModel)
        { }
    }
}
