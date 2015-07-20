using Detector.Extractors.EF60;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TestBase;

namespace Detector.Extractors.EF60Tests
{
    [TestClass]
    public class DataContextDeclarationExtractorTests
    {
        [TestMethod]
        public async Task DetectsDbContextClass_When_ThereIsOneDbContextDeclaredInAProject()
        {
            //Arrange
            Solution EF60_NWSolution = await new RoslynSolutionGenerator().GetEF60_NWSolutionAsync();
            var target = new DataContextDeclarationExtractor();

            //Act
            await target.FindDataContextDeclarationsAsync(EF60_NWSolution);

            //Assert
            Assert.IsTrue(target.DataContextDeclarations.Count == 1);
        }
    }
}
