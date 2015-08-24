using Detector.Models.ORM.ORMTools;
using System;

namespace Detector.Main.DetectionRules
{
    class InefficientLazyLoadingDetectionRule<T> : DetectionRule where T : ORMToolType
    {
        protected override Func<bool> GetRuleFunction()
        {
            throw new NotImplementedException();
        }
    }
}
