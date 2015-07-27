using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TestBase.Stubs;
using Detector.Models.ORM;
using Detector.Extractors.Base;
using Microsoft.CodeAnalysis;
using TestBase.RoslynSolutionGenerators;

namespace Detector.Extractors.EF602.Tests
{
    [TestClass]
    public class DatabaseAccessingMethodCallsExtractorTests
    {
        /// <summary>
        /// EF60_NW project contains LINQ to Entities queries in Query and Method syntax
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task ExtractsDatabaseAccessingMethodCalls_When_EF60_NWProjectIsUsed()
        {
            //Arrange
            Solution solution = await new RoslynSolutionGenerator().GetSolutionAsync(@"..\..\..\..\ProjectsUnderTest\EF60_NW\EF60_NW.sln");

            var progressIndicator = new ProgressStub();

            var context = new ContextStub<EntityFramework>();
            var dataContextDecExtr = new DataContextDeclarationExtractor(context);
            await dataContextDecExtr.FindDataContextDeclarationsAsync(solution, progressIndicator);
            var dbEntityExtractor = new DatabaseEntityDeclarationExtractor(context);
            await dbEntityExtractor.FindDatabaseEntityDeclarationsAsync(solution, progressIndicator);
            var target = new DatabaseAccessingMethodCallExtractor(context);

            //Act
            await target.FindDatabaseAccessingMethodCallsAsync(solution, progressIndicator);
            var result = target.DatabaseAccessingMethodCalls;

            //Assert
            Assert.IsTrue(target.DatabaseAccessingMethodCalls.Count == 7);
        }
    }
}

