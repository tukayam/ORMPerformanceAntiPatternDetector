using Detector.Models.AntiPatterns;
using Detector.Models.Base;
using Detector.Models.ORM.DatabaseAccessingMethodCalls;
using Detector.Models.ORM.DatabaseEntities;
using Detector.Models.ORM.ORMTools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Detector.Main.DetectionRules
{
    public class ExcessiveDataDetectionRule<T> : DetectionRule where T : ORMToolType
    {
        protected override Func<bool> GetRuleFunction()
        {
            return CodeExecutionPathHasEagerFetchingQueryAndNotAllFetchedEntitiesAreUsed;
        }

        private bool CodeExecutionPathHasEagerFetchingQueryAndNotAllFetchedEntitiesAreUsed()
        {
            IEnumerable<DatabaseAccessingMethodCallStatement<T>> databaseAccessingMethodCalls = CodeExecutionPath.Models.OfType<DatabaseAccessingMethodCallStatement<T>>();

            IEnumerable<DatabaseEntityVariableRelatedEntityCallStatement<T>> databaseEntityVariableRelatedEntityCalls = CodeExecutionPath.Models.OfType<DatabaseEntityVariableRelatedEntityCallStatement<T>>();

            foreach (var dbAccessingMethodCall in databaseAccessingMethodCalls)
            {
                // Eager Loading is configured
                if (dbAccessingMethodCall.DoesEagerLoad)
                {
                    // what is checked here? if there are no loops? (considering onebyone should check loops)
                    if (!databaseEntityVariableRelatedEntityCalls.Any(x => x.CalledDatabaseEntityVariable == dbAccessingMethodCall.AssignedVariable))
                    {
                        this.DetectedAntiPatterns.Add(new ExcessiveDataAntiPattern(CodeExecutionPath, dbAccessingMethodCall));

                        return true;
                    }
                }
            }

            return false;
        }
    }
}
