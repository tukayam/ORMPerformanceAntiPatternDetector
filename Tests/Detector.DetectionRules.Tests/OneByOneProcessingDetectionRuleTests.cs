using Microsoft.VisualStudio.TestTools.UnitTesting;
using Detector.Models;
using Detector.Models.ORM;
using Detector.Models.Others;
using System.Collections.Generic;

namespace Detector.DetectionRules.Tests
{
    [TestClass]
    public class OneByOneProcessingDetectionRuleTests
    {
        OneByOneProcessingDetectionRule<LINQToSQL> target;

        [TestInitialize]
        public void Initialize()
        {
            target = new OneByOneProcessingDetectionRule<LINQToSQL>();
        }

        [TestMethod]
        public void DetectsOneByOneProcessingAntiPattern_When_ThereIsOneQueryInTheTreeThatDoesLazyLoadingAndRelatedEntityIsUsedOnAssignedVariable()
        {
            //Arrange
            #region  Create tree with method declaration node as the root method

            var rootMethodDeclaration = new MethodDeclaration("Main", null);
            var rootNode = new ORMModelNode(rootMethodDeclaration);
            var tree = new ORMModelTree(rootNode);

            #endregion

            #region Add database accessing method call as a child for the root node
                        
            var customerEntityDeclaration = new DatabaseEntityDeclaration<LINQToSQL>("Customer");
            var orderEntityDeclaration = new DatabaseEntityDeclaration<LINQToSQL>("Order");

            var entityDeclarationsUsedInQuery = new List<DatabaseEntityDeclaration<LINQToSQL>>() { customerEntityDeclaration };
            DatabaseQueryVariable databaseQueryVariable = null;

            var dbQuery = new DatabaseQuery<LINQToSQL>("(from c in dc.Customers where c.Id=1 select c)", entityDeclarationsUsedInQuery, databaseQueryVariable);
            var dbAccessingMethodCall = new DatabaseAccessingMethodCallStatement<LINQToSQL>(dbQuery, null);

            //Make the database accessing method call lazy loading
            var entityDeclarationsLoadedByDbCall = new List<DatabaseEntityDeclaration<LINQToSQL>>() { customerEntityDeclaration };
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

            #region Add call to loaded database entity variable to retrieve related object

            var databaseEntityVariableForOrder = new DatabaseEntityVariable<LINQToSQL>("order1", null);
            var databaseEntityRelatedObjectCall = new DatabaseEntityVariableRelatedEntityCallStatement<LINQToSQL>(databaseEntityObject, null);

            var databaseEntityRelatedObjectCallNode = new ORMModelNode(databaseEntityRelatedObjectCall);
            rootNode.ChildNodes.Add(databaseEntityRelatedObjectCallNode);

            #endregion

            //Act
            bool result = target.AppliesToModelTree(tree);

            //Assert
            Assert.IsTrue(result);
        }
    }
}
