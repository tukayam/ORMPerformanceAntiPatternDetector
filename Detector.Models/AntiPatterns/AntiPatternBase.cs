using Detector.Models.Base;

namespace Detector.Models.AntiPatterns
{
    public abstract class AntiPatternBase
    {
        protected CodeExecutionPath CodeExecutionPath { get; private set; }
        protected Model ReferenceModel { get; private set; }

        public AntiPatternBase(CodeExecutionPath codeExecutionPath, Model referenceModel)
        {
            CodeExecutionPath = codeExecutionPath;
            ReferenceModel = referenceModel;
        }
    }
}
