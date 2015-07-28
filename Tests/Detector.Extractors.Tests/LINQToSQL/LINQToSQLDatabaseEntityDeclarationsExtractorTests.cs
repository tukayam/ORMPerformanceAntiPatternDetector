using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Linq;
using Detector.Extractors.LINQToSQL40;
using Detector.Extractors.Tests.Helpers.RoslynSolutionGenerators;
using Detector.Models.ORM;
using Detector.Extractors.Base;
using TestBase.Stubs;
using Detector.Models.ORM.ORMTools;

namespace Detector.Extractors.Tests
{
    [TestClass]
    public class LINQToSQLDatabaseEntityDeclarationsExtractorOnRoslynSolutionTests
    {
        LINQToSQLDatabaseEntityDeclarationExtractor target;

        [TestMethod]
        public async Task DetectsDatabaseEntityDeclarations_When_RoslynComplexSolutionIsUsed()
        {
            //Arrange
            var solGen = new RoslynComplexSolutionGenerator();
            var solution = solGen.GetRoslynSolution();

            //ToDo: change to use TargetBuilder
            Context<LINQToSQL> context = new ContextStub<LINQToSQL>();

            target = new LINQToSQLDatabaseEntityDeclarationExtractor(context);

            var progressIndicator = new ProgressStub();
            //Act
            await target.FindDatabaseEntityDeclarationsAsync(solution, progressIndicator);
            var result = target.DatabaseEntityDeclarations;

            //Assert
            Assert.IsTrue(result.Count() == 3);
            Assert.IsTrue(result.ToList().Exists(x => x.Name == "Customer"));
            Assert.IsTrue(result.ToList().Exists(x => x.Name == "Order"));
            Assert.IsTrue(result.ToList().Exists(x => x.Name == "OrderItem"));
        }
    }
}
