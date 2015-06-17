using System;
using Detector.Models.ORM;
using System.Collections.Generic;
using Detector.Models.Base;
using System.Linq;
using Detector.Models.AntiPatterns;

namespace Detector.Main.DetectionRules
{
    public class OneByOneProcessingDetectionRule<T> : DetectionRule where T : ORMToolType
    {
        protected override Func<bool> GetRuleFunction()
        {
            return TreeHasLazyFetchingDatabaseAccessingMethodCallAndRelatedEntitiesAreCalledOnReturnedObject;
        }

        public bool TreeHasLazyFetchingDatabaseAccessingMethodCallAndRelatedEntitiesAreCalledOnReturnedObject()
        {
            List<NodeBase> databaseAccessingMethodCalls = ORMModelTree.RootNode.ChildNodes.OfType<DatabaseAccessingMethodCallStatement<T>>().ToList();
            List<NodeBase> databaseEntityVariableRelatedEntityCalls = ORMModelTree.RootNode.ChildNodes.OfType<DatabaseEntityVariableRelatedEntityCallStatement<T>>().ToList();

            foreach (NodeBase item in databaseAccessingMethodCalls)
            {
                DatabaseAccessingMethodCallStatement<T> dbAccessingMethodCall = (DatabaseAccessingMethodCallStatement<T>)item.Model;
                if (!dbAccessingMethodCall.DoesEagerLoad)
                {
                    NodeBase relatedEntityCall = databaseEntityVariableRelatedEntityCalls.Where(x => ((DatabaseEntityVariableRelatedEntityCallStatement<T>)x.Model).CalledDatabaseEntityVariable == dbAccessingMethodCall.AssignedVariable).FirstOrDefault();
                    if (relatedEntityCall != null)
                    {
                        this.DetectedAntiPatterns.Add(
                            new OneByOneProcessingAntiPattern(relatedEntityCall.Model, ORMModelTree));

                        return true;
                    }
                }
            }

            return false;

        }
    }
}
