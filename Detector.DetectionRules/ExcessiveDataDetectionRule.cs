using Detector.Models;
using Detector.Models.Base;
using Detector.Models.ORM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Detector.DetectionRules
{
    public class ExcessiveDataDetectionRule<T> : DetectionRule where T : ORMToolType
    {
        protected override Func<bool> GetRuleFunction()
        {
            return TreeHasEagerFetchingQueryAndNotAllFetchedEntitiesAreUsed;
        }

        private bool TreeHasEagerFetchingQueryAndNotAllFetchedEntitiesAreUsed()
        {
            List<NodeBase> databaseAccessingMethodCalls = ORMModelTree.RootNode.ChildNodes.OfType<DatabaseAccessingMethodCallStatement<T>>().ToList();

            List<NodeBase> databaseEntityVariableRelatedEntityCalls = ORMModelTree.RootNode.ChildNodes.OfType<DatabaseEntityVariableRelatedEntityCallStatement<T>>().ToList();

            foreach (NodeBase item in databaseAccessingMethodCalls)
            {
                DatabaseAccessingMethodCallStatement<T> dbAccessingMethodCall = (DatabaseAccessingMethodCallStatement<T>)item.Model;
                if (dbAccessingMethodCall.DoesEagerLoad)
                {
                    if (!databaseEntityVariableRelatedEntityCalls.Exists(x => ((DatabaseEntityVariableRelatedEntityCallStatement<T>)x.Model).CalledDatabaseEntityVariable == dbAccessingMethodCall.AssignedVariable))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
