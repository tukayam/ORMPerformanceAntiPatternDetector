using System;
using Detector.Models.ORM.ORMTools;
using System.Collections.Generic;
using Detector.Models.ORM.DatabaseAccessingMethodCalls;
using Detector.Models.ORM.DatabaseEntities;
using System.Linq;
using Detector.Models.AntiPatterns;

namespace Detector.Main.DetectionRules
{
    class InefficientLazyLoadingDetectionRule<T> : DetectionRule where T : ORMToolType
    {
        protected override Func<bool> GetRuleFunction()
        {
            throw new NotImplementedException();
            //return PathHasLazyLoadingDbAccessingMethodCallAndRelatedEntitiesAreCalled;
        }

        public bool PathHasLazyLoadingDbAccessingMethodCallAndRelatedEntitiesAreCalled()
        {
            IEnumerable<DatabaseAccessingMethodCallStatement<T>> databaseAccessingMethodCalls = CodeExecutionPath.Models.OfType<DatabaseAccessingMethodCallStatement<T>>();

            IEnumerable<DatabaseEntityVariableRelatedEntityCallStatement<T>> databaseEntityVariableRelatedEntityCalls = CodeExecutionPath.Models.OfType<DatabaseEntityVariableRelatedEntityCallStatement<T>>();

            foreach (var dbAccessingMethodCall in databaseAccessingMethodCalls)
            {
                // check for Lazy Loading configured
                if (!dbAccessingMethodCall.DoesEagerLoad)
                {
                    // TODO Find objects of these database entities in code execution paths
                    //if (databaseEntityVariableRelatedEntityCalls.Any(x => x.CalledDatabaseEntityVariable == dbAccessingMethodCall.AssignedVariable))
                    //{
                    //    this.DetectedAntiPatterns.Add(new InefficientLazyLoadingAntiPattern(CodeExecutionPath, dbAccessingMethodCall));

                    return true;
                    //}
                }
            }

            return false;
        }
    }
}
