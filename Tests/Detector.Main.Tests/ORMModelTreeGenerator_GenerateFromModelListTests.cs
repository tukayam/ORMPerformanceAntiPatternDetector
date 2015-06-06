using Detector.Main;
using Detector.Models;
using Detector.Models.Base;
using Detector.Models.ORM;
using Detector.Models.Others;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Detector.Extractors.Tests
{
    [TestClass]
    public class LINQToSQLORMModelTreeGeneratorTests
    {
        ORMModelTreeGenerator<LINQToSQL> target;

        [TestInitialize]
        public void Initialize()
        {
            target = new ORMModelTreeGenerator<LINQToSQL>();
        }

        [TestMethod]
        public void OrdersNodesCorrectly_When_NodesForSameDocumentAreSent()
        {
            //Arrange
            var methodCompilationInfo = new CompilationInfo(300);
            var methodDec = new MethodDeclaration("GetSomething", methodCompilationInfo);
            List<MethodDeclaration> methodDeclarations = new List<MethodDeclaration>() { methodDec };

            var dataContextCompilationInfo = new CompilationInfo(500);
            dataContextCompilationInfo.SetParentMethodDeclaration(methodDec);
            var dataContextDec = new DataContextInitializationStatement<LINQToSQL>(dataContextCompilationInfo);
            var dataContextInitializationStatements = new List<DataContextInitializationStatement<LINQToSQL>>() { dataContextDec };

            var dataAccessingMethodCallOnQueryCompilationInfo = new CompilationInfo(1000);
            dataAccessingMethodCallOnQueryCompilationInfo.SetParentMethodDeclaration(methodDec);
            var dataAccessingMethodCallOnQueryDec = new DatabaseAccessingMethodCallStatementOnQueryDeclaration<LINQToSQL>(null, dataAccessingMethodCallOnQueryCompilationInfo);
            List<DatabaseAccessingMethodCallStatementOnQueryDeclaration<LINQToSQL>> dataAccessingMethodCallOnQueryDecs = new List<DatabaseAccessingMethodCallStatementOnQueryDeclaration<LINQToSQL>>() { dataAccessingMethodCallOnQueryDec };

            List<ModelBase> models = new List<ModelBase>();
            models.AddRange(dataAccessingMethodCallOnQueryDecs);
            models.AddRange(dataContextInitializationStatements);
            models.AddRange(methodDeclarations);

            //Act           
            ORMModelTree tree = target.GenerateFromModelList(models);

            //Assert
            Assert.IsTrue(tree.RootNode == methodDec);
            Assert.IsTrue(tree.RootNode.ChildNodes[0] == dataContextDec);
            Assert.IsTrue(tree.RootNode.ChildNodes[1] == dataAccessingMethodCallOnQueryDec);
        }
    }
}
