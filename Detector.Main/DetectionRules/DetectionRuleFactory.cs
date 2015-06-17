using Detector.Models.ORM;
using System.Collections.Generic;

namespace Detector.Main.DetectionRules
{
    class DetectionRuleFactory<T> where T : ORMToolType
    {
        public List<DetectionRule> GetDetectionRules()
        {
            return new List<DetectionRule>()
            {
                new ExcessiveDataDetectionRule<T>(),
                new OneByOneProcessingDetectionRule<T>()
            };
        }
    }
}
