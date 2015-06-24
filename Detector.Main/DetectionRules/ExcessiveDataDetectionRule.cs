﻿using Detector.Models;
using Detector.Models.AntiPatterns;
using Detector.Models.Base;
using Detector.Models.ORM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Detector.Main.DetectionRules
{
    public class ExcessiveDataDetectionRule<T> : DetectionRule where T : ORMToolType
    {
        protected override Func<bool> GetRuleFunction()
        {
            return TreeHasEagerFetchingQueryAndNotAllFetchedEntitiesAreUsed;
        }

        private bool TreeHasEagerFetchingQueryAndNotAllFetchedEntitiesAreUsed()
        {
            IEnumerable<NodeBase> databaseAccessingMethodCalls = ORMModelTree.RootNode.ChildNodes.OfType<DatabaseAccessingMethodCallStatement<T>>();

            List<NodeBase> databaseEntityVariableRelatedEntityCalls = ORMModelTree.RootNode.ChildNodes.OfType<DatabaseEntityVariableRelatedEntityCallStatement<T>>().ToList();

            foreach (NodeBase item in databaseAccessingMethodCalls)
            {
                DatabaseAccessingMethodCallStatement<T> dbAccessingMethodCall = (DatabaseAccessingMethodCallStatement<T>)item.Model;
                if (dbAccessingMethodCall.DoesEagerLoad)
                {
                    if (!databaseEntityVariableRelatedEntityCalls.Exists(x => ((DatabaseEntityVariableRelatedEntityCallStatement<T>)x.Model).CalledDatabaseEntityVariable == dbAccessingMethodCall.AssignedVariable))
                    {
                        this.DetectedAntiPatterns.Add(new ExcessiveDataAntiPattern(dbAccessingMethodCall, ORMModelTree));

                        return true;
                    }
                }
            }

            return false;
        }
    }
}