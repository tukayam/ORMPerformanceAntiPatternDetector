using Detector.Models.AntiPatterns;
using Detector.Models.Base;
using System;
using System.Collections.Generic;

namespace Detector.Main.DetectionRules
{
    public abstract class DetectionRule
    {
        protected CodeExecutionPath CodeExecutionPath { get; private set; }
        
        public bool AppliesToModelTree(CodeExecutionPath codeExecutionPath)
        {
            this.CodeExecutionPath = codeExecutionPath;
            return GetRuleFunction().Invoke();
        }

        public List<AntiPatternBase> DetectedAntiPatterns { get; protected set; }

        protected abstract Func<bool> GetRuleFunction();

        public DetectionRule()
        {
            this.DetectedAntiPatterns = new List<AntiPatternBase>();
        }
    }
}
