using Detector.Extractors.Base;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            //Act
            await target.FindDataContextDeclarationsAsync(EF60_NWSolution);

            //Assert
            Assert.IsTrue(target.DataContextDeclarations.Count == 1);

            foreach (var item in target.DataContextDeclarations)
            {
                Assert.IsTrue(item.Name == "NWDbContext");
            }

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

            //Act
            await target.FindDataContextDeclarationsAsync(EF60_NWSolution);

            //Assert
            Assert.IsTrue(target.DataContextDeclarations.Count == 15);
        }
    }
}
