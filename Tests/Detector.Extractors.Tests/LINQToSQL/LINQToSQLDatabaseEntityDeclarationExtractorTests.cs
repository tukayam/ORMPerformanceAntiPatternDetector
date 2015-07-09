using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.CodeAnalysis;
using System.Threading.Tasks;
using Detector.Extractors.LINQToSQL40;
using Detector.Extractors.Tests.Helpers.RoslynSolutionGenerators;

namespace Detector.Extractors.Tests
{
    [TestClass]
    public class LINQToSQLDatabaseEntityDeclarationExtractorTests
    {
        LINQToSQLDatabaseEntityDeclarationExtractorOnOneDocument target;

        [TestInitialize]
        public void Initialize()
        {
            target = new LINQToSQLDatabaseEntityDeclarationExtractorOnOneDocument();
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
