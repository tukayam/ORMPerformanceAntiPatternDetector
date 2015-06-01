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
        ORMModelTreeGenerator target;

        [TestInitialize]
        public void Initialize()
        {
            target = new ORMModelTreeGenerator();
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
            ORMModelTree tree = target.GenerateFromModelList(models);

            //Assert
            Assert.IsTrue(tree.RootNode == methodDec);
            Assert.IsTrue(tree.RootNode.ChildNodes[0] == dataContextDec);
            Assert.IsTrue(tree.RootNode.ChildNodes[1] == dataAccessingMethodCallOnQueryDec);
        }
    }
}
