using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Detector.Models.Others;
using Detector.Models.Base;
using Detector.Models.ORM;

namespace Detector.Models.Tests.ORM
{
    /// <summary>
    /// Summary description for ORMModelTree_GenerateFromModelListTests
    /// </summary>
    [TestClass]
    public class ORMModelTree_GenerateFromModelListTests
    {
        ORMModelTree target;

        [TestInitialize]
        public void Initialize()
        {
            target = new ORMModelTree(null);
        }

        [TestMethod]
        public void OrdersNodesCorrectly_When_AllModelsAreFromSameFile()
        {
            //Arrange
            var methodCompilationInfo = new CompilationInfo("C:\\something.cs", "something.cs", 5);
            var methodDec = new MethodDeclaration("GetSomething", methodCompilationInfo);
            List<MethodDeclaration> methodDeclarations = new List<MethodDeclaration>() { methodDec };

            var dataContextCompilationInfo = new CompilationInfo("C:\\something.cs", "something.cs", 7);
            var dataContextDec = new DataContextInitializationStatement(dataContextCompilationInfo);
            List<DataContextInitializationStatement> dataContextInitializationStatements = new List<DataContextInitializationStatement>() { dataContextDec };

            var dataAccessingMethodCallOnQueryCompilationInfo = new CompilationInfo("C:\\something.cs", "something.cs", 10);
            var dataAccessingMethodCallOnQueryDec = new DatabaseAccessingMethodCallStatementOnQueryDeclaration<LINQToSQL>(null, dataAccessingMethodCallOnQueryCompilationInfo);
            List<DatabaseAccessingMethodCallStatementOnQueryDeclaration<LINQToSQL>> dataAccessingMethodCallOnQueryDecs = new List<DatabaseAccessingMethodCallStatementOnQueryDeclaration<LINQToSQL>>() { dataAccessingMethodCallOnQueryDec };

            List<ModelBase> models = new List<ModelBase>();
            models.AddRange(dataAccessingMethodCallOnQueryDecs);
            models.AddRange(dataContextInitializationStatements);
            models.AddRange(methodDeclarations);

            //Act
            target.GenerateFromModelList(models);

            //Assert
            Assert.IsTrue(target.RootNode == methodDec);
            Assert.IsTrue(target.RootNode.ChildNodes[0] == dataContextDec);
            Assert.IsTrue(target.RootNode.ChildNodes[1] == dataAccessingMethodCallOnQueryDec);
        }
    }
}
