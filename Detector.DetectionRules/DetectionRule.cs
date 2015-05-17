using Detector.Models;
using System;

namespace Detector.Extractors.DetectionRules
{
    public abstract class DetectionRule
    {
        protected ORMModelTree SyntaxTree { get; private set; }
        
        public bool AppliesToSyntaxTree(ORMModelTree ORMSyntaxTree)
        {
            this.SyntaxTree = ORMSyntaxTree;
            return GetRuleFunction().Invoke();
        }

        public abstract Func<bool> GetRuleFunction();
    }
}
