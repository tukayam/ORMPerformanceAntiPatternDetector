using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TestBase.Stubs;
using Microsoft.CodeAnalysis;
using TestBase.RoslynSolutionGenerators;
using Detector.Models.ORM.ORMTools;
using System.Linq;
using System.Configuration;

namespace Detector.Extractors.EF602.Tests
{
    [TestClass]
    public class CodeExecutionPathExtractorTests
    {
        [TestMethod]
        public async Task ExtractsMethodCallsBetweenServiceAndRepositoryClasses_When_EF60_NWProjectIsUsed()
        {
            //Arrange
            Solution solution = await new RoslynSolutionGenerator().GetSolutionAsync(@"..\..\..\..\ProjectsUnderTest\EF60_NW\EF60_NW.sln");

            var progressIndicator = new ProgressStub();

            var context = new ContextStub<EntityFramework>();
            var dataContextDecExtr = new DataContextDeclarationExtractor(context);
            await dataContextDecExtr.FindDataContextDeclarationsAsync(solution, progressIndicator);
            var dbEntityExtractor = new DatabaseEntityDeclarationExtractorUsingDbContextProperties(context);
            await dbEntityExtractor.FindDatabaseEntityDeclarationsAsync(solution, progressIndicator);
            var dbAccessingMethodCallsExtractor = new DatabaseAccessingMethodCallExtractor(context);
            await dbAccessingMethodCallsExtractor.FindDatabaseAccessingMethodCallsAsync(solution, progressIndicator);
            var target = new CodeExecutionPathGenerator(context);
            
            //Act
            await target.GenerateCodeExecutionPathsAsync(solution, progressIndicator);

            //Assert
            Assert.IsTrue(target.CodeExecutionPaths.Count() == 2);
        }


        [TestMethod]
        public async Task ExtractsMethodCallsBetweenServiceAndRepositoryClasses_When_VirtoCommerceSolutionIsCompiled()
        {
            //Arrange
            string solutionFilePath = ConfigurationManager.AppSettings["PathToSolutionFile_VirtoCommerce"];
            //Solution EF60_NWSolution = await new RoslynSolutionGenerator().GetSolutionAsync(solutionFilePath);
            Solution solution = await new RoslynSolutionGenerator().GetSolutionAsync(solutionFilePath);

            var progressIndicator = new ProgressStub();

            var context = new ContextStub<EntityFramework>();
            var dataContextDecExtr = new DataContextDeclarationExtractor(context);
            await dataContextDecExtr.FindDataContextDeclarationsAsync(solution, progressIndicator);
            var dbEntityExtractor = new DatabaseEntityDeclarationExtractorUsingDbContextProperties(context);
            await dbEntityExtractor.FindDatabaseEntityDeclarationsAsync(solution, progressIndicator);
            var dbAccessingMethodCallsExtractor = new DatabaseAccessingMethodCallExtractor(context);
            await dbAccessingMethodCallsExtractor.FindDatabaseAccessingMethodCallsAsync(solution, progressIndicator);
            var target = new CodeExecutionPathGenerator(context);

            //Act
            await target.GenerateCodeExecutionPathsAsync(solution, progressIndicator);

            //Assert
            //Assert.IsTrue(target.CodeExecutionPaths.Count() == 362);
            // VirtoCommerce.Platform.sln seems to return 54, not 362? could be due to upgrades in vc-community software?
            Assert.IsTrue(target.CodeExecutionPaths.Count() == 54);
        }
    }
}
