using Detector.Models;
using Detector.Models.ORM;
using Detector.Models.Others;
using System.Collections.Generic;

namespace Detector.Main.Tests.DetectionRules.Helpers
{
    class ORMModelTreeGenerator
    {
        public ORMModelTree Tree;

        DatabaseEntityDeclaration<LINQToSQL> customerEntityDeclaration = new DatabaseEntityDeclaration<LINQToSQL>("Customer");
        DatabaseEntityDeclaration<LINQToSQL> orderEntityDeclaration = new DatabaseEntityDeclaration<LINQToSQL>("Order");
        DatabaseAccessingMethodCallStatement<LINQToSQL> dbAccessingMethodCall;
        DatabaseEntityVariableDeclaration<LINQToSQL> databaseEntityObject = new DatabaseEntityVariableDeclaration<LINQToSQL>("customer1", null);

        public ORMModelTreeGenerator()
        {
            var rootMethodDeclaration = new MethodDeclaration("Main", null);
            var rootNode = new ORMModelNode(rootMethodDeclaration);
            Tree = new ORMModelTree(rootNode);
        }

        public ORMModelTreeGenerator WithLazyLoadingDatabaseAccessingMethodCall()
        {
            dbAccessingMethodCall = GetDatabaseAccessingMethodCall();

            var entityDeclarationsLoadedByDbCall = new List<DatabaseEntityDeclaration<LINQToSQL>>() { customerEntityDeclaration };
            dbAccessingMethodCall.SetLoadedEntityDeclarations(entityDeclarationsLoadedByDbCall);

            var dbAccessingMethodCallNode = new ORMModelNode(dbAccessingMethodCall);
            Tree.RootNode.ChildNodes.Add(dbAccessingMethodCallNode);
            return this;
        }

        public ORMModelTreeGenerator WithEagerLoadingDatabaseAccessingMethodCall()
        {
            dbAccessingMethodCall = GetDatabaseAccessingMethodCall();
            var entityDeclarationsLoadedByDbCall = new List<DatabaseEntityDeclaration<LINQToSQL>>() { customerEntityDeclaration, orderEntityDeclaration };
            dbAccessingMethodCall.SetLoadedEntityDeclarations(entityDeclarationsLoadedByDbCall);

            var dbAccessingMethodCallNode = new ORMModelNode(dbAccessingMethodCall);
            Tree.RootNode.ChildNodes.Add(dbAccessingMethodCallNode);

            return this;
        }

        private DatabaseAccessingMethodCallStatement<LINQToSQL> GetDatabaseAccessingMethodCall()
        {
            var entityDeclarationsUsedInQuery = new ModelCollection<DatabaseEntityDeclaration<LINQToSQL>>() { customerEntityDeclaration };
            DatabaseQueryVariable databaseQueryVariable = null;

            var dbQuery = new DatabaseQuery<LINQToSQL>("(from c in dc.Customers where c.Id=1 select c)", entityDeclarationsUsedInQuery, databaseQueryVariable);
            return new DatabaseAccessingMethodCallStatement<LINQToSQL>(dbQuery, null);

        }
        
        public void WithDatabaseAccessingMEthodCallLoadingVariable(DatabaseEntityVariableDeclaration<LINQToSQL> dbEntityVariable)
        {
            var customerEntityDeclaration = new DatabaseEntityDeclaration<LINQToSQL>("Customer");
            var orderEntityDeclaration = new DatabaseEntityDeclaration<LINQToSQL>("Order");

            var entityDeclarationsUsedInQuery = new ModelCollection<DatabaseEntityDeclaration<LINQToSQL>>() { customerEntityDeclaration };
            DatabaseQueryVariable databaseQueryVariable = null;

            var dbQuery = new DatabaseQuery<LINQToSQL>("(from c in dc.Customers where c.Id=1 select c)", entityDeclarationsUsedInQuery, databaseQueryVariable);
            var dbAccessingMethodCall = new DatabaseAccessingMethodCallStatement<LINQToSQL>(dbQuery, null);

            var entityDeclarationsLoadedByDbCall = new List<DatabaseEntityDeclaration<LINQToSQL>>() { customerEntityDeclaration, orderEntityDeclaration };
            dbAccessingMethodCall.SetLoadedEntityDeclarations(entityDeclarationsLoadedByDbCall);

            var dbAccessingMethodCallNode = new ORMModelNode(dbAccessingMethodCall);
            Tree.RootNode.ChildNodes.Add(dbAccessingMethodCallNode);
        }

        public ORMModelTreeGenerator WithDatabaseEntityVariableAssignedByDatabaseAccessingMethodCall()
        {
            dbAccessingMethodCall.SetAssignedVariable(databaseEntityObject);
            var dbEntityObjectNode = new ORMModelNode(databaseEntityObject);
            Tree.RootNode.ChildNodes.Add(dbEntityObjectNode);

            return this;
        }

        public ORMModelTreeGenerator WithCallToRelatedEntityOnDatabaseEntityVariableAssignedByDatabaseAccessingMethodCall()
        {
            var databaseEntityRelatedObjectCall = new DatabaseEntityVariableRelatedEntityCallStatement<LINQToSQL>(databaseEntityObject, null);

            var databaseEntityRelatedObjectCallNode = new ORMModelNode(databaseEntityRelatedObjectCall);
            Tree.RootNode.ChildNodes.Add(databaseEntityRelatedObjectCallNode);

            return this;
        }
    }
}
