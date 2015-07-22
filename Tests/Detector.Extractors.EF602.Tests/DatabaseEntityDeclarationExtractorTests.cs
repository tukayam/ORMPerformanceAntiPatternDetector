using Detector.Extractors.Base;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestBase.RoslynSolutionGenerators;
using TestBase.Stubs;

namespace Detector.Extractors.EF602.Tests
{
    [TestClass]
    public class DatabaseEntityDeclarationExtractorTests
    {
        [TestMethod]
        public async Task DetectsThreeDatabaseEntityDeclarations_When_EF60_NWProjectIsUsed()
        {
            //Arrange  
            Solution EF60_NWSolution = await new RoslynSolutionGenerator().GetSolutionAsync(@"..\..\..\..\ProjectsUnderTest\EF60_NW\EF60_NW.sln");

            Context<EntityFramework> context = new ContextStub<EntityFramework>();
            var dataContextDecExtr = new DataContextDeclarationExtractor(context);
            await dataContextDecExtr.FindDataContextDeclarationsAsync(EF60_NWSolution);

            var target = new DatabaseEntityDeclarationExtractor(context);

            //Act
            await target.FindDatabaseEntityDeclarationsAsync(EF60_NWSolution);

            //Assert
            Assert.IsTrue(target.DatabaseEntityDeclarations.Count == 3);

            IEnumerable<string> dbEntityNames = target.DatabaseEntityDeclarations.Select(d => d.Name);
            Assert.IsTrue(dbEntityNames.Contains("Customer"));
            Assert.IsTrue(dbEntityNames.Contains("Order"));
            Assert.IsTrue(dbEntityNames.Contains("OrderItem"));

            Assert.IsTrue(context.DatabaseEntityDeclarations == target.DatabaseEntityDeclarations);
        }
    }
}
