using Microsoft.VisualStudio.TestTools.UnitTesting;
using Detector.Models;
using Detector.Models.ORM;
using Detector.Models.Others;
using System.Collections.Generic;

namespace Detector.DetectionRules.Tests
{
    [TestClass]
    public class ExcessiveDataDetectionRuleTests
    {
        ExcessiveDataDetectionRule<LINQToSQL> target;

        [TestInitialize]
        public void Initialize()
        {
            target = new ExcessiveDataDetectionRule<LINQToSQL>();
        }

        [TestMethod]
        public void DetectsExcessiveDataAntiPattern_When_ThereIsOneQueryInTheTreeThatFetchesEagerlyAndEagerlyFetchedEntityIsNotUsed()
        {
            //Arrange
            #region  Create tree with method declaration node as the root method

            var rootMethodDeclaration = new MethodDeclaration("Main", null);
            var rootNode = new ORMModelNode(rootMethodDeclaration);
            var tree = new ORMModelTree(rootNode);

            #endregion

            #region Add database accessing method call as a child for the root node

            //This database accessing method call does eager loading while fetching Customer. Related Order objects are also loaded for Customer object.
            var customerEntityDeclaration = new DatabaseEntityDeclaration<LINQToSQL>("Customer");
            var orderEntityDeclaration = new DatabaseEntityDeclaration<LINQToSQL>("Order");

            var entityDeclarationsUsedInQuery = new List<DatabaseEntityDeclaration<LINQToSQL>>() { customerEntityDeclaration };
            DatabaseQueryVariable databaseQueryVariable = null;

            var dbQuery = new DatabaseQuery<LINQToSQL>("(from c in dc.Customers where c.Id=1 select c)", entityDeclarationsUsedInQuery, databaseQueryVariable);
            var dbAccessingMethodCall = new DatabaseAccessingMethodCallStatement<LINQToSQL>(dbQuery, null);

            var entityDeclarationsLoadedByDbCall = new List<DatabaseEntityDeclaration<LINQToSQL>>() { customerEntityDeclaration, orderEntityDeclaration };
            dbAccessingMethodCall.SetLoadedEntityDeclarations(entityDeclarationsLoadedByDbCall);

            var dbAccessingMethodCallNode = new ORMModelNode(dbAccessingMethodCall);
            rootNode.ChildNodes.Add(dbAccessingMethodCallNode);
            #endregion

            #region Add Database Entity Object as a variable that is filled in by the database accessing method call

            var databaseEntityObject = new DatabaseEntityVariable<LINQToSQL>("customer1", null);
            dbAccessingMethodCall.SetAssignedVariable(databaseEntityObject);
            var dbEntityObjectNode = new ORMModelNode(databaseEntityObject);
            rootNode.ChildNodes.Add(dbEntityObjectNode);

            #endregion

            //Act
            bool result = target.AppliesToModelTree(tree);

            //Assert
            Assert.IsTrue(result);
        }
    }
}
