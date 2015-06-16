using Detector.Models;
using System;

namespace Detector.DetectionRules
{
    public abstract class DetectionRule
    {
        protected ORMModelTree ORMModelTree { get; private set; }
        
        public bool AppliesToModelTree(ORMModelTree ORMModelTree)
        {
            this.ORMModelTree = ORMModelTree;
            return GetRuleFunction().Invoke();
        }

        protected abstract Func<bool> GetRuleFunction();
    }
}
