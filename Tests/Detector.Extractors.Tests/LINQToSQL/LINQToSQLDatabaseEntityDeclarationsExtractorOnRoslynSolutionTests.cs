﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Linq;
using Detector.Extractors.LINQToSQL40;
using Detector.Extractors.Tests.Helpers.RoslynSolutionGenerators;

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

            target = new LINQToSQLDatabaseEntityDeclarationExtractor();

            //Act
            var result = await target.ExtractAsync(solution);


            //Assert
            Assert.IsTrue(result.Count() == 3);
            Assert.IsTrue(result.ToList().Exists(x => x.Name == "Customer"));
            Assert.IsTrue(result.ToList().Exists(x => x.Name == "Order"));
            Assert.IsTrue(result.ToList().Exists(x => x.Name == "OrderItem"));
        }
    }
}
