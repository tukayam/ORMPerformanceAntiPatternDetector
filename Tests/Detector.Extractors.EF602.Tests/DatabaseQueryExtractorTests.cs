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
    public class DatabaseQueryExtractorTests
    {
        /// <summary>
        /// EF60_NW project contains LINQ to Entities queries in Query and Method syntax
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task ExtractsDatabaseQueries_When_EF60_NWProjectIsUsed()
        {
            //Arrange
            Solution solution = await new RoslynSolutionGenerator().GetSolutionAsync(@"..\..\..\..\ProjectsUnderTest\EF60_NW\EF60_NW.sln");

            var context = new ContextStub<EntityFramework>();
            var dataContextDecExtr = new DataContextDeclarationExtractor(context);
            await dataContextDecExtr.FindDataContextDeclarationsAsync(solution);
            var dbEntityExtractor = new DatabaseEntityDeclarationExtractor(context);
            await dbEntityExtractor.FindDatabaseEntityDeclarationsAsync(solution);
            var target = new DatabaseQueryExtractor(context);

            //Act
            await target.FindDatabaseQueriesAsync(solution);
            var result = target.DatabaseQueries;

            //Assert
            Assert.IsTrue(target.DatabaseQueries.Count == 7);
        }

        [TestMethod]
        public async Task ExtractsDatabaseQueries_When_VirtoCommerceUsed()
        {
            //Arrange
            Solution solution = await new RoslynSolutionGenerator().GetSolutionAsync(@"..\..\..\..\..\..\vc-community\PLATFORM\VirtoCommerce.WebPlatform.sln");

            var context = new ContextStub<EntityFramework>();
            var dataContextDecExtr = new DataContextDeclarationExtractor(context);
            await dataContextDecExtr.FindDataContextDeclarationsAsync(solution);
            var dbEntityExtractor = new DatabaseEntityDeclarationExtractor(context);
            await dbEntityExtractor.FindDatabaseEntityDeclarationsAsync(solution);
            var target = new DatabaseQueryExtractor(context);

            //Act
            await target.FindDatabaseQueriesAsync(solution);
            var result = target.DatabaseQueries;

            //Assert
            Assert.IsTrue(target.DatabaseQueries.Count == 7);
        }
    }
}
