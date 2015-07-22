using Microsoft.VisualStudio.TestTools.UnitTesting;
using Detector.Models.ORM;
using Detector.Main.DetectionRules;

namespace Detector.Main.Tests.DetectionRules
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
        public void DetectsOneByOneProcessingAntiPattern_When_ThereIsOneQueryInTheTreeThatDoesLazyLoadingAndRelatedEntityIsUsedInALoop()
        {
            //Arrange
            //var ormModelTreeGenerator = new ORMModelTreeGenerator()
            // .WithLazyLoadingDatabaseAccessingMethodCall()
            // .WithDatabaseEntityVariableAssignedByDatabaseAccessingMethodCall()
            // .WithCallToRelatedEntityOnDatabaseEntityVariableAssignedByDatabaseAccessingMethodCall();

            ////Act
            //bool result = target.AppliesToModelTree(ormModelTreeGenerator.Tree);

            ////Assert
            //Assert.IsTrue(result);
        }
    }
}
