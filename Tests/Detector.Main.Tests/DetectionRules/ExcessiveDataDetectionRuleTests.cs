using Microsoft.VisualStudio.TestTools.UnitTesting;
using Detector.Models.ORM;
using Detector.Main.DetectionRules;

namespace Detector.Main.Tests.DetectionRules
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
            //ORMModelTreeGenerator ormModelTreeGenerator = new ORMModelTreeGenerator()
            //    .WithEagerLoadingDatabaseAccessingMethodCall();

            ////Act
            //bool result = target.AppliesToModelTree(ormModelTreeGenerator.Tree);

            ////Assert
            //Assert.IsTrue(result);
        }
    }
}
