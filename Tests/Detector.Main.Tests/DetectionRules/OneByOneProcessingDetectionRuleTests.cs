using Microsoft.VisualStudio.TestTools.UnitTesting;
using Detector.Main.DetectionRules;
using Detector.Main.Tests.Stubs;
using Detector.Main.Tests.DetectionRules.Helpers;

namespace Detector.Main.Tests.DetectionRules
{
    [TestClass]
    public class OneByOneProcessingDetectionRuleTests
    {
        OneByOneProcessingDetectionRule<FakeORMToolType> target;

        [TestInitialize]
        public void Initialize()
        {
            target = new OneByOneProcessingDetectionRule<FakeORMToolType>();
        }

        [TestMethod]
        public void DetectsOneByOneProcessingAntiPattern_When_ThereIsOneQueryInTheTreeThatDoesLazyLoadingAndRelatedEntityIsUsedInALoop()
        {
            //Arrange
            var codeExecutionPath = new CodeExecutionPathGenerator()
             .WithLazyLoadingDatabaseAccessingMethodCall()
             .WithDatabaseEntityVariableAssignedByDatabaseAccessingMethodCall()
             .WithCallToRelatedEntityOnDatabaseEntityVariableAssignedByDatabaseAccessingMethodCall()
             .Build();

            ////Act
           bool result = target.AppliesToModelTree(codeExecutionPath);

            ////Assert
            Assert.IsTrue(result);
        }
    }
}
