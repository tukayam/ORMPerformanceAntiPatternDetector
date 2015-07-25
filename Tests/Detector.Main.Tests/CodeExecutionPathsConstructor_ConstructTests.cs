using Microsoft.VisualStudio.TestTools.UnitTesting;
using Detector.Extractors.Base;
using Moq;
using System.Collections.Generic;
using Detector.Models;
using Detector.Models.ORM;
using Detector.Main.Tests.Stubs;
using Detector.Main.Tests.RoslynSolutionGenerators;

namespace Detector.Main.Tests
{
    [TestClass]
    public class CodeExecutionPathsConstructor_ConstructTests
    {
        [TestMethod]
        public void ReturnsSetWithOneCodeExecutionPath_When_ThereIsOnePathContainingDBAccessingMethodCall()
        {
            //Arrange
           // var solGenerator = new RoslynSolutionBuilder().GetRoslynSolution();


            //Act

            //Assert

        }

        class TargetBuilder
        {
            private CodeExecutionPathsConstructor<FakeORMToolType> _target;
            private ExtractorFactory<FakeORMToolType> _extractorFactory;

            private Mock<DatabaseAccessingMethodCallExtractor<FakeORMToolType>> _mockDatabaseAccessingMethodCallsExtractor;

            public TargetBuilder()
            {
                var mockExtractorsFactory = new Mock<ExtractorFactory<FakeORMToolType>>();

                _mockDatabaseAccessingMethodCallsExtractor = new Mock<DatabaseAccessingMethodCallExtractor<FakeORMToolType>>();

                mockExtractorsFactory.Setup(m => m.GetDatabaseAccessingMethodCallsExtractor()).Returns(_mockDatabaseAccessingMethodCallsExtractor.Object);

                _extractorFactory = mockExtractorsFactory.Object;                
                _target = new CodeExecutionPathsConstructor<FakeORMToolType>(_extractorFactory);
            }

            public void WithOnePathContainingADbAccessingMethodCall()
            {
               // var dbAccessingMethodCall = new DatabaseAccessingMethodCallStatement<LINQToSQL>();
            }

        }
    }
}
