using Detector.Models;
using Detector.Models.ORM.Base;
using System;
using System.Collections.Generic;

namespace Detector.Extractors.DetectionRules
{
    /// <summary>
    ///  
    /// </summary>
    public class ExcessiveDataDetectionRule : DetectionRule
    {
        public override Func<bool> GetRuleFunction()
        {
            return TreeHasEagerFetchingQueryAndNotAllFetchedEntitiesAreUsed;
        }

        private bool TreeHasEagerFetchingQueryAndNotAllFetchedEntitiesAreUsed()
        {
            bool result = false;
            IEnumerable<ModelBase> databaseAccessingMethodCalls = SyntaxTree.Nodes.FindAll(n => n is DatabaseAccessingMethodCallStatement);

            foreach (DatabaseAccessingMethodCallStatement databaseAccessingMethodCall in databaseAccessingMethodCalls)
            {
                if (databaseAccessingMethodCall.Query is EagerFetchingQuery)
                {
                    EagerFetchingQuery eagerFetchingQuery = databaseAccessingMethodCall.Query as EagerFetchingQuery;
                    DatabaseEntityObject databaseEntityObjectSetByCall = databaseAccessingMethodCall.DatabaseEntityObject;
                    if (databaseEntityObjectSetByCall != null)
                    {
                        var callsOnDatabaseEntityObjectInTree = SyntaxTree.Nodes.FindAll(n => n is DatabaseEntityObjectCallStatement
                         && ((DatabaseEntityObjectCallStatement)n).DatabaseEntityObject == databaseEntityObjectSetByCall);

                        foreach (var call in callsOnDatabaseEntityObjectInTree)
                        {
                            if (call is DatabaseEntityObjectRelatedEntitySelectCallStatement)
                            {
                                DatabaseEntityObject relatedEntity = ((DatabaseEntityObjectRelatedEntitySelectCallStatement)call).RelatedEntityObject;

                                if ((eagerFetchingQuery.FetchedEntities as List<DatabaseEntityDeclaration>).Contains(relatedEntity.DatabaseEntityDeclaration))
                                {
                                    result = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
