using Microsoft.VisualStudio.TestTools.UnitTesting;
using Detector.Main.DetectionRules;
using Detector.Main.Tests.Stubs;
using Detector.Main.Tests.DetectionRules.Helpers;

namespace Detector.Main.Tests.DetectionRules
{
    [TestClass]
    public class ExcessiveDataDetectionRuleTests
    {
        ExcessiveDataDetectionRule<FakeORMToolType> target;

        [TestInitialize]
        public void Initialize()
        {
            target = new ExcessiveDataDetectionRule<FakeORMToolType>();
        }

        [TestMethod]
        public void DetectsExcessiveDataAntiPattern_When_ThereIsOneQueryInTheTreeThatFetchesEagerlyAndEagerlyFetchedEntityIsNotUsed()
        {
            //Arrange
            var codeExecutionPath = new CodeExecutionPathGenerator()
                .WithEagerLoadingDatabaseAccessingMethodCall()
                .Build();

            //Act
            bool result = target.AppliesToModelTree(codeExecutionPath);

            //Assert
            Assert.IsTrue(result);
        }
    }
}
