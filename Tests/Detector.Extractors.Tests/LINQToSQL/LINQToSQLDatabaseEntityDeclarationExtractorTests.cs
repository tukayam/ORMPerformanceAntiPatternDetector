using Microsoft.VisualStudio.TestTools.UnitTesting;
using Detector.Extractors.DatabaseEntities;
using Microsoft.CodeAnalysis;
using Detector.Extractors.Tests.RoslynSolutionGenerators;
using System.Threading.Tasks;

namespace Detector.Extractors.Tests
{
    [TestClass]
    public class LINQToSQLDatabaseEntityDeclarationExtractorTests
    {
        LINQToSQLDatabaseEntityDeclarationExtractor target;

        [TestInitialize]
        public void Initialize()
        {
            target = new LINQToSQLDatabaseEntityDeclarationExtractor();
        }

        [TestMethod]
        public async Task DetectsDatabaseEntityDeclaration_When_DocumentWithLINQToSQLEntityClassRootIsVisited()
        {
            //Arrange
            var solGen = new RoslynSimpleSolutionGenerator();
            SyntaxNode root = await solGen.GetRootNodeForEntityDocument();

            //Act
            target.Visit(root);

            //Assert
            Assert.IsTrue(target.DatabaseEntityDeclarations.Count == 1);
        }

      
    }
}
