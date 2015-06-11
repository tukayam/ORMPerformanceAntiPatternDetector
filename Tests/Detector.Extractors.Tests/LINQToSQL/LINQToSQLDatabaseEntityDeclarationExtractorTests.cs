using Microsoft.VisualStudio.TestTools.UnitTesting;
using Detector.Extractors.DatabaseEntities;
using Microsoft.CodeAnalysis;
using Detector.Extractors.Tests.RoslynSolutionGenerators;

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
        public void DetectsDatabaseEntityDeclaration_When_DocumentWithLINQToSQLEntityClassRootIsVisited()
        {
            //Arrange
            var solGen = new RoslynSimpleSolutionGenerator();
            SyntaxNode root = solGen.GetRootNodeForEntityDocument();

            //Act
            target.Visit(root);

            //Assert
            Assert.IsTrue(target.DatabaseEntityDeclarations.Count == 1);
        }
    }
}
