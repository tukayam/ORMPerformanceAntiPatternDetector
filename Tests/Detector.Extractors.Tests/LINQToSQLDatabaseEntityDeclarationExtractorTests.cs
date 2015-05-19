using Microsoft.VisualStudio.TestTools.UnitTesting;
using Detector.Extractors.DatabaseEntities;
using Detector.Extractors.Tests.Helper;
using Microsoft.CodeAnalysis;

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
            var solGen = new RoslynSolutionGenerator();
            SyntaxNode root = solGen.GetRootForEntityDocument();

            //Act
            target.Visit(root);

            //Assert
            Assert.IsTrue(target.DatabaseEntityDeclarations.Count == 1);
        }
    }
}
