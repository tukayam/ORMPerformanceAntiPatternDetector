using Detector.Extractors.DatabaseEntities;
using Detector.Extractors.Tests.Helper;
using Detector.Models.ORM;
using Detector.Models.Others;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Detector.Extractors.Tests
{
    [TestClass]
    public class LINQToSQLORMModelTreeExtractorTests
    {
        LINQToSQLORMModelTreeExtractor target;
        DatabaseEntityDeclarationsExtractor<LINQToSQL> _databaseEntityDeclarationsExtractor;
        [TestInitialize]
        public void Initialize()
        {
            var entityDeclarations = new List<DatabaseEntityDeclaration<LINQToSQL>>();
            entityDeclarations.Add(new DatabaseEntityDeclaration<LINQToSQL>("L2S_Northwind.Employee"));
            var mock = new Mock<DatabaseEntityDeclarationsExtractor<LINQToSQL>>();
            mock.Setup(foo => foo.EntityDeclarations).Returns(entityDeclarations);
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

            target = new LINQToSQLORMModelTreeExtractor(semanticModelForMainClass, _databaseEntityDeclarationsExtractor);

            //Act
            target.Visit(solGenerator.GetRootNodeForMainDocument());

            IEnumerable<DatabaseAccessingMethodCallStatement<LINQToSQL>> result = target.DatabaseAccessingMethodCalls;

            //Assert
            Assert.IsTrue(result.Count() == 1);
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

            target = new LINQToSQLORMModelTreeExtractor(semanticModelForMainClass, _databaseEntityDeclarationsExtractor);

            //Act
            target.Visit(solGenerator.GetRootNodeForMainDocument());

            List<DatabaseAccessingMethodCallStatement<LINQToSQL>> result = target.DatabaseAccessingMethodCalls.ToList();

            //Assert
            Assert.IsTrue(result[0].DatabaseQuery.EntityDeclarations.Count() == 1);
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

            target = new LINQToSQLORMModelTreeExtractor(semanticModelForMainClass, _databaseEntityDeclarationsExtractor);

            //Act
            target.Visit(solGenerator.GetRootNodeForMainDocument());
            IEnumerable<DatabaseAccessingMethodCallStatement<LINQToSQL>> result = target.DatabaseAccessingMethodCalls;

            //Assert
            Assert.IsTrue(result.Count() == 1);
        }

        [TestMethod]
        public void DetectsDatabaseAccessingMethodCall_When_DBAccessingMethodIsInAForEachLoop()
        {
            //Arrange
            string textToPlaceInMainMethod = @" 
									NorthWindDataClassesDataContext dc = new NorthWindDataClassesDataContext();
                                    var employees = (from e in dc.Employees
											where (e.EmployeeID == empId)
											select e);
                                    
                                    List<int> employeeIds=new List<int>();
                                    foreach(var emp in employees)
                                    {
                                        employeeIds.Add(emp.EmployeeID);
                                    }
            
									return employeeIds;";

            var solGenerator = new RoslynSolutionGenerator(textToPlaceInMainMethod);

            SemanticModel semanticModelForMainClass = solGenerator.GetSemanticModelForMainClass();

            target = new LINQToSQLORMModelTreeExtractor(semanticModelForMainClass, _databaseEntityDeclarationsExtractor);

            //Act
            target.Visit(solGenerator.GetRootNodeForMainDocument());

            IEnumerable<DatabaseAccessingForeachLoopDeclaration<LINQToSQL>> result = target.DatabaseAccessingForeachLoopDeclarations;

            //Assert
            Assert.IsTrue(result.Count() == 1);
        }

        [TestMethod]
        public void DetectsDatabaseAccessingMethodCall_When_DBAccessingMethodIsInAForLoop()
        {
            //Arrange
            string textToPlaceInMainMethod = @" 
									NorthWindDataClassesDataContext dc = new NorthWindDataClassesDataContext();
                                    var employees = (from e in dc.Employees
											where (e.EmployeeID == empId)
											select e);
                                    
                                    List<int> employeeIds=new List<int>();
                                    for (int i = 0; i < employees.Count(); i++)
                                    {
                                        employeeIds.Add(employees.ToList()[i].EmployeeID);
                                    }
            
									return employeeIds;";

            var solGenerator = new RoslynSolutionGenerator(textToPlaceInMainMethod);

            SemanticModel semanticModelForMainClass = solGenerator.GetSemanticModelForMainClass();

            target = new LINQToSQLORMModelTreeExtractor(semanticModelForMainClass, _databaseEntityDeclarationsExtractor);

            //Act
            target.Visit(solGenerator.GetRootNodeForMainDocument());

            IEnumerable<DatabaseAccessingForLoopDeclaration<LINQToSQL>> result = target.DatabaseAccessingForLoopDeclarations;

            //Assert
            Assert.IsTrue(result.Count() == 1);
        }

        [TestMethod]
        public void DetectsWhileLoop_When_ThereIsAWhileLoop()
        {
            //Arrange
            string textToPlaceInMainMethod = @" 
									NorthWindDataClassesDataContext dc = new NorthWindDataClassesDataContext();
                                    var employees = (from e in dc.Employees
											where (e.EmployeeID == empId)
											select e);
                                    
                                    List<int> employeeIds=new List<int>();
                                    int i = employees.Count() - 1;
                                    while(i>=0)
                                    {
                                        employeeIds.Add(employees.ToList()[i].EmployeeID);
                                        i--;
                                    }
            
									return employeeIds;";

            var solGenerator = new RoslynSolutionGenerator(textToPlaceInMainMethod);

            SemanticModel semanticModelForMainClass = solGenerator.GetSemanticModelForMainClass();

            target = new LINQToSQLORMModelTreeExtractor(semanticModelForMainClass, _databaseEntityDeclarationsExtractor);

            //Act
            target.Visit(solGenerator.GetRootNodeForMainDocument());

            IEnumerable<WhileLoopDeclaration> result = target.WhileLoopDeclarations;

            //Assert
            Assert.IsTrue(result.Count() == 1);
        }
    }
}
