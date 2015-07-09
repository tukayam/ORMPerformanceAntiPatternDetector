using Microsoft.VisualStudio.TestTools.UnitTesting;
using Detector.Extractors.DatabaseEntities;
using Detector.Models.ORM;
using System.Collections.Generic;
using Moq;
using Detector.Extractors.LINQToSQL40;
using Detector.Models.Others;

namespace Detector.Extractors.Tests
{
    [TestClass]
    public class LINQToSQLLoopDeclarationExtractorTests
    {
        LINQToSQLLoopDeclarationExtractor target;
        DatabaseEntityDeclarationExtractor<LINQToSQL> _databaseEntityDeclarationsExtractor;

        [TestInitialize]
        public void Initialize()
        {
            var entityDeclarations = new ModelCollection<DatabaseEntityDeclaration<LINQToSQL>>();
            entityDeclarations.Add(new DatabaseEntityDeclaration<LINQToSQL>("L2S_Northwind.Employee"));
            var mock = new Mock<DatabaseEntityDeclarationExtractor<LINQToSQL>>();
            mock.Setup(foo => foo.DatabaseEntityDeclarations).Returns(entityDeclarations);
            _databaseEntityDeclarationsExtractor = mock.Object;
        }        

        //[TestMethod]
        //public void DetectsDatabaseAccessingMethodCall_When_DBAccessingMethodIsInAForEachLoop()
        //{
        //    //Arrange
        //    string textToPlaceInMainMethod = @" 
								//	NorthWindDataClassesDataContext dc = new NorthWindDataClassesDataContext();
        //                            var employees = (from e in dc.Employees
								//			where (e.EmployeeID == empId)
								//			select e);
                                    
        //                            List<int> employeeIds=new List<int>();
        //                            foreach(var emp in employees)
        //                            {
        //                                employeeIds.Add(emp.EmployeeID);
        //                            }
            
								//	return employeeIds;";

        //    var solGenerator = new RoslynSolutionGenerator(textToPlaceInMainMethod);

        //    SemanticModel semanticModelForMainClass = solGenerator.GetSemanticModelForMainClass();

        //    target = new LINQToSQLLoopDeclarationExtractor(semanticModelForMainClass, _databaseEntityDeclarationsExtractor);

        //    //Act
        //    target.Visit(solGenerator.GetRootNodeForMainDocument());

        //    IEnumerable<DatabaseAccessingForeachLoopDeclaration<LINQToSQL>> result = target.DatabaseAccessingForeachLoopDeclarations;

        //    //Assert
        //    Assert.IsTrue(result.Count() == 1);
        //}

        //[TestMethod]
        //public void DetectsDatabaseAccessingMethodCall_When_DBAccessingMethodIsInAForLoop()
        //{
        //    //Arrange
        //    string textToPlaceInMainMethod = @" 
								//	NorthWindDataClassesDataContext dc = new NorthWindDataClassesDataContext();
        //                            var employees = (from e in dc.Employees
								//			where (e.EmployeeID == empId)
								//			select e);
                                    
        //                            List<int> employeeIds=new List<int>();
        //                            for (int i = 0; i < employees.Count(); i++)
        //                            {
        //                                employeeIds.Add(employees.ToList()[i].EmployeeID);
        //                            }
            
								//	return employeeIds;";

        //    var solGenerator = new RoslynSolutionGenerator(textToPlaceInMainMethod);

        //    SemanticModel semanticModelForMainClass = solGenerator.GetSemanticModelForMainClass();

        //    target = new LINQToSQLLoopDeclarationExtractor(semanticModelForMainClass, _databaseEntityDeclarationsExtractor);

        //    //Act
        //    target.Visit(solGenerator.GetRootNodeForMainDocument());

        //    IEnumerable<DatabaseAccessingForLoopDeclaration<LINQToSQL>> result = target.DatabaseAccessingForLoopDeclarations;

        //    //Assert
        //    Assert.IsTrue(result.Count() == 1);
        //}

        //[TestMethod]
        //public void DetectsWhileLoop_When_ThereIsAWhileLoop()
        //{
        //    //Arrange
        //    string textToPlaceInMainMethod = @" 
								//	NorthWindDataClassesDataContext dc = new NorthWindDataClassesDataContext();
        //                            var employees = (from e in dc.Employees
								//			where (e.EmployeeID == empId)
								//			select e);
                                    
        //                            List<int> employeeIds=new List<int>();
        //                            int i = employees.Count() - 1;
        //                            while(i>=0)
        //                            {
        //                                employeeIds.Add(employees.ToList()[i].EmployeeID);
        //                                i--;
        //                            }
            
								//	return employeeIds;";

        //    var solGenerator = new RoslynSolutionGenerator(textToPlaceInMainMethod);

        //    SemanticModel semanticModelForMainClass = solGenerator.GetSemanticModelForMainClass();

        //    target = new LINQToSQLLoopDeclarationExtractor(semanticModelForMainClass, _databaseEntityDeclarationsExtractor);

        //    //Act
        //    target.Visit(solGenerator.GetRootNodeForMainDocument());

        //    IEnumerable<WhileLoopDeclaration> result = target.WhileLoopDeclarations;

        //    //Assert
        //    Assert.IsTrue(result.Count() == 1);
        //}
    }
}
