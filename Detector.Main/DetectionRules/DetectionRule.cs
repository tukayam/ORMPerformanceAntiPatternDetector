using Detector.Models;
using Detector.Models.AntiPatterns;
using System;
using System.Collections.Generic;

namespace Detector.Main.DetectionRules
{
    public abstract class DetectionRule
    {
        protected ORMModelTree ORMModelTree { get; private set; }
        
        public bool AppliesToModelTree(ORMModelTree ORMModelTree)
        {
            this.ORMModelTree = ORMModelTree;
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
