using Microsoft.VisualStudio.TestTools.UnitTesting;
using Detector.Models.ORM;
using Detector.Extractors.DatabaseEntities;
using System.Collections.Generic;
using Moq;
using Microsoft.CodeAnalysis;
using Detector.Extractors.Tests.Helper;

namespace Detector.Extractors.Tests
{
    [TestClass]
    public class LINQToSQLDatabaseAccessingMethodCallExtractorTests
    {
        LINQToSQLDatabaseAccessingMethodCallExtractor target;
        DatabaseEntityDeclarationExtractor<LINQToSQL> _databaseEntityDeclarationsExtractor;

        [TestInitialize]
        public void Initialize()
        {
            var entityDeclarations = new List<DatabaseEntityDeclaration<LINQToSQL>>();
            entityDeclarations.Add(new DatabaseEntityDeclaration<LINQToSQL>("L2S_Northwind.Employee"));
            var mock = new Mock<DatabaseEntityDeclarationExtractor<LINQToSQL>>();
            mock.Setup(foo => foo.DatabaseEntityDeclarations).Returns(entityDeclarations);
            _databaseEntityDeclarationsExtractor = mock.Object;
        }

        [TestMethod]
        public void DetectsDatabaseAccessingMethodCall_When_DBAccessingMethodIsOnSameSentenceAsQuery()
        {
            //Arrange
            string textToPlaceInMainMethod = @" 
									var dc = new NorthWindDataClassesDataContext();								
									
									return (from e in dc.Employees
											where (e.EmployeeID == empId)
											select e).SingleOrDefault<Employee>();";

            var solGenerator = new RoslynSolutionGenerator(textToPlaceInMainMethod);

            SemanticModel semanticModelForMainClass = solGenerator.GetSemanticModelForMainClass();

            target = new LINQToSQLDatabaseAccessingMethodCallExtractor(semanticModelForMainClass, _databaseEntityDeclarationsExtractor);

            //Act
            target.Visit(solGenerator.GetRootNodeForMainDocument());

            List<DatabaseAccessingMethodCallStatement<LINQToSQL>> result = target.DatabaseAccessingMethodCalls;

            //Assert
            Assert.IsTrue(result.Count == 1);
        }

        [TestMethod]
        public void DetectsDatabaseAccessingMethodCallWithCorrectDatabaseEntities_When_DBAccessingMethodIsOnSameSentenceAsQuery()
        {
            //Arrange
            string textToPlaceInMainMethod = @" 
									var dc = new NorthWindDataClassesDataContext();								
									
									return (from e in dc.Employees
											where (e.EmployeeID == empId)
											select e).SingleOrDefault<Employee>();";

            var solGenerator = new RoslynSolutionGenerator(textToPlaceInMainMethod);

            SemanticModel semanticModelForMainClass = solGenerator.GetSemanticModelForMainClass();

            target = new LINQToSQLDatabaseAccessingMethodCallExtractor(semanticModelForMainClass, _databaseEntityDeclarationsExtractor);

            //Act
            target.Visit(solGenerator.GetRootNodeForMainDocument());

            List<DatabaseAccessingMethodCallStatement<LINQToSQL>> result = target.DatabaseAccessingMethodCalls;

            //Assert
            Assert.IsTrue(result[0].DatabaseQuery.EntityDeclarations.Count == 1);
            Assert.IsTrue(result[0].DatabaseQuery.EntityDeclarations[0].Name == "L2S_Northwind.Employee");
        }

        [TestMethod]
        public void DetectsDatabaseAccessingMethodCall_When_DBAccessingMethodIsOnQueryVariable()
        {
            //Arrange
            string textToPlaceInMainMethod = @" 
									NorthWindDataClassesDataContext dc = new NorthWindDataClassesDataContext();
                                    var query = (from e in dc.Employees
											where (e.EmployeeID == empId)
											select e);
									return query.SingleOrDefault<Employee>();";

            var solGenerator = new RoslynSolutionGenerator(textToPlaceInMainMethod);

            SemanticModel semanticModelForMainClass = solGenerator.GetSemanticModelForMainClass();

            target = new LINQToSQLDatabaseAccessingMethodCallExtractor(semanticModelForMainClass, _databaseEntityDeclarationsExtractor);

            //Act
            target.Visit(solGenerator.GetRootNodeForMainDocument());
            List<DatabaseAccessingMethodCallStatement<LINQToSQL>> result = target.DatabaseAccessingMethodCalls;

            //Assert
            Assert.IsTrue(result.Count == 1);
        }
    }
}
