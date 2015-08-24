using Detector.Models.ORM.ORMTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detector.Main.DetectionRules
{
    class SameQueryExecutionDetectionRule<T> : DetectionRule where T : ORMToolType
    {
        protected override Func<bool> GetRuleFunction()
        {
            throw new NotImplementedException();
        }
    }
}
