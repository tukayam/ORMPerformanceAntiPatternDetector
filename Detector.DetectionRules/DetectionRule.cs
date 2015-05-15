using Detector.Models;
using System;
using System.Collections.Generic;

namespace Detector.Extractors.DetectionRules
{
    public abstract class DetectionRule
    {
        protected ORMSyntaxTree SyntaxTree { get; private set; }
        
        public bool AppliesToSyntaxTree(ORMSyntaxTree ORMSyntaxTree)
        {
            this.SyntaxTree = ORMSyntaxTree;
            return GetRuleFunction().Invoke();
        }

        public abstract Func<bool> GetRuleFunction();
    }
}
