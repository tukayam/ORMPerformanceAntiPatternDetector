using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Detector.Extractors.Tests.RoslynSolutionGenerators;
using System.Linq;

namespace Detector.Extractors.Tests
{
    [TestClass]
    public class LINQToSQLDatabaseEntityDeclarationsExtractorOnRoslynSolutionTests
    {
        LINQToSQLDatabaseEntityDeclarationsExtractorOnRoslynSolution target;

        [TestMethod]
        public async Task DetectsDatabaseEntityDeclarations_When_RoslynComplexSolutionIsUsed()
        {
            //Arrange
            var solGen = new RoslynComplexSolutionGenerator();          
            var solution = solGen.GetRoslynSolution();

            target = new LINQToSQLDatabaseEntityDeclarationsExtractorOnRoslynSolution();

            //Act
            var result = await target.ExtractFromSolution(solution);


            //Assert
            Assert.IsTrue(result.Count() == 3);
            Assert.IsTrue(result.ToList().Exists(x => x.Name == "Customer"));
            Assert.IsTrue(result.ToList().Exists(x => x.Name == "Order"));
            Assert.IsTrue(result.ToList().Exists(x => x.Name == "OrderItem"));
        }
    }
}
