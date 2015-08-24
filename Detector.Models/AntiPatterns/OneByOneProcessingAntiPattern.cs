using Detector.Models.Base;

namespace Detector.Models.AntiPatterns
{
    public sealed class OneByOneProcessingAntiPattern : AntiPatternBase
    {
        public OneByOneProcessingAntiPattern(CodeExecutionPath codeExecutionPath, Model referenceModel)
            : base(codeExecutionPath, referenceModel)
        { }
    }
}
