using Detector.Models;
using Detector.Models.ORM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Detector.Extractors.DetectionRules
{
    /// <summary>
    ///  
    /// </summary>
    public class ExcessiveDataDetectionRule<T> : DetectionRule where T : ORMToolType
    {
        public override Func<bool> GetRuleFunction()
        {
            //return TreeHasEagerFetchingQueryAndNotAllFetchedEntitiesAreUsed;

            return Temp;
        }

        private bool Temp()
        {
            return true;
        }

        //private bool TreeHasEagerFetchingQueryAndNotAllFetchedEntitiesAreUsed()
        //{
        //    bool result = false;
        //    IEnumerable<Models.ModelBase> databaseAccessingMethodCalls = SyntaxTree.Nodes.FindAll(n => n is DatabaseAccessingMethodCallStatement<T>);

        //    foreach (DatabaseAccessingMethodCallStatement<T> databaseAccessingMethodCall in databaseAccessingMethodCalls)
        //    {
        //        //TODO: If the query does not mention multiple entities, it is still possible that it fetches eagerly..
        //        var entityTypesQuerySelects = databaseAccessingMethodCall.DatabaseQuery.EntityDeclarations.ToList();
        //        if (entityTypesQuerySelects.Count() > 0)
        //        {
        //            //DatabaseEntityObject<T> databaseEntityObjectSetByCall = databaseAccessingMethodCall.DatabaseEntityObject;
        //            //if (databaseEntityObjectSetByCall != null)
        //            //{
        //            //    var callsOnDatabaseEntityObjectInTree = SyntaxTree.Nodes.FindAll(n => n is DatabaseEntityObjectCallStatement<T>
        //            //     && ((DatabaseEntityObjectCallStatement<T>)n).DatabaseEntityObject == databaseEntityObjectSetByCall);

        //            //    foreach (var call in callsOnDatabaseEntityObjectInTree)
        //            //    {
        //            //        if (call is DatabaseEntityObjectRelatedEntitySelectCallStatement)
        //            //        {
        //            //            DatabaseEntityObject relatedEntity = ((DatabaseEntityObjectRelatedEntitySelectCallStatement)call).RelatedEntityObject;

        //            //            if (entityTypesQuerySelects.Contains(relatedEntity.DatabaseEntityDeclaration))
        //            //            {
        //            //                result = true;
        //            //                break;
        //            //            }
        //            //        }
        //            //    }
        //            //}
        //        }
        //    }

        //    return result;
        //}
    }
}
