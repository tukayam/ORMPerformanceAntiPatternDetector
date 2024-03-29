﻿using Detector.Extractors.Base;
using Detector.Models.ORM.ORMTools;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using TestBase.RoslynSolutionGenerators;
using TestBase.Stubs;

namespace Detector.Extractors.EF602.Tests
{
    [TestClass]
    public class DataContextDeclarationExtractorTests
    {
        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task DetectsDbContextClasses_When_EF60_NWProjectIsCompiled()
        {
            //Arrange  
            Solution EF60_NWSolution = await new RoslynSolutionGenerator().GetSolutionAsync(@"..\..\..\..\ProjectsUnderTest\EF60_NW\EF60_NW.sln");

            //ToDo: Use target builder instead
            Context<EntityFramework> context = new ContextStub<EntityFramework>();

            var target = new DataContextDeclarationExtractor(context);

            var progressIndicator = new ProgressStub();

            //Act
            await target.FindDataContextDeclarationsAsync(EF60_NWSolution, progressIndicator);

            //Assert
            var item = target.DataContextDeclarations.First();

            Assert.IsTrue(target.DataContextDeclarations.Count == 1);           
            Assert.IsTrue(item.Name == "NWDbContext");
            Assert.IsTrue(target.DataContextDeclarations.Count == 1);
            Assert.IsTrue(context.DataContextDeclarations == target.DataContextDeclarations);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task DetectsDbContextClasses_When_VirtoCommerceSolutionIsCompiled()
        {
            //Arrange  
            Solution EF60_NWSolution = await new RoslynSolutionGenerator().GetSolutionAsync(@"..\..\..\..\..\..\vc-community\PLATFORM\VirtoCommerce.WebPlatform.sln");
            //ToDo: Use target builder instead
            Context<EntityFramework> context = new ContextStub<EntityFramework>();
            var target = new DataContextDeclarationExtractor(context);

            var progressIndicator = new ProgressStub();

            //Act
            await target.FindDataContextDeclarationsAsync(EF60_NWSolution, progressIndicator);

            //Assert
            Assert.IsTrue(target.DataContextDeclarations.Count == 15);
        }
    }
}
