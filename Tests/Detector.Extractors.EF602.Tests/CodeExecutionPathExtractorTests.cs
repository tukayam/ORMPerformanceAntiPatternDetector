using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TestBase.Stubs;
using Microsoft.CodeAnalysis;
using TestBase.RoslynSolutionGenerators;
using Detector.Models.ORM.ORMTools;
using System.Linq;

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
    }
}
