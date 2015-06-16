using Microsoft.VisualStudio.TestTools.UnitTesting;
using Detector.Models.ORM;
using System.Collections.Generic;
using Detector.Extractors.Tests.RoslynSolutionGenerators;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Detector.Extractors.Tests
{
    [TestClass]
    public class LINQToSQLDatabaseQueryExtractorTests
    {
        LINQToSQLDatabaseQueryExtractor target;

        [TestMethod]
        public async Task ExtractsOneDatabaseQueryObject_When_QueryVariableIsDeclared()
        {
            //Arrange
            List<DatabaseEntityDeclaration<LINQToSQL>> entities = new List<DatabaseEntityDeclaration<LINQToSQL>>();
            entities.Add(new DatabaseEntityDeclaration<LINQToSQL>("L2S_Northwind.Employee"));

            string textToPlaceInMainMethod = @" 
									NorthWindDataClassesDataContext dc = new NorthWindDataClassesDataContext();
                                    var query = (from e in dc.Employees
											where (e.EmployeeID == empId)
											select e);
									return query.SingleOrDefault<Employee>();";

            var solGenerator = new RoslynSimpleSolutionGenerator(textToPlaceInMainMethod);

            SemanticModel semanticModelForMainClass =await solGenerator.GetSemanticModelForMainClass();

            target = new LINQToSQLDatabaseQueryExtractor(semanticModelForMainClass, entities);

            //Act
            target.Visit(await solGenerator.GetRootNodeForMainDocument());

            //Assert
            Assert.IsTrue(target.DatabaseQueries.ToList().Count == 1);
        }

        [TestMethod]
        public async Task ExtractsDatabaseQueryWithQueryVariable_When_QueryVariableIsDeclared()
        {
            //Arrange
            List<DatabaseEntityDeclaration<LINQToSQL>> entities = new List<DatabaseEntityDeclaration<LINQToSQL>>();
            entities.Add(new DatabaseEntityDeclaration<LINQToSQL>("L2S_Northwind.Employee"));

            string textToPlaceInMainMethod = @" 
									NorthWindDataClassesDataContext dc = new NorthWindDataClassesDataContext();
                                    var query = (from e in dc.Employees
											where (e.EmployeeID == empId)
											select e);
									return query.SingleOrDefault<Employee>();";

            var solGenerator = new RoslynSimpleSolutionGenerator(textToPlaceInMainMethod);

            SemanticModel semanticModelForMainClass = await solGenerator.GetSemanticModelForMainClass();

            target = new LINQToSQLDatabaseQueryExtractor(semanticModelForMainClass, entities);

            //Act
            target.Visit(await solGenerator.GetRootNodeForMainDocument());
            var result = target.DatabaseQueries.ToList().First();

            //Assert
            Assert.IsTrue(result.DatabaseQueryVariable.VariableName == "query");
        }

        [TestMethod]
        public async Task ExtractsOneQueryWithCorrectAmountOfUsedEntities_When_NoQueryVariableIsDeclared()
        {
            //Arrange
            List<DatabaseEntityDeclaration<LINQToSQL>> entities = new List<DatabaseEntityDeclaration<LINQToSQL>>();
            entities.Add(new DatabaseEntityDeclaration<LINQToSQL>("L2S_Northwind.Employee"));

            string textToPlaceInMainMethod = @" 
									NorthWindDataClassesDataContext dc = new NorthWindDataClassesDataContext();
                                     
									return (from e in dc.Employees
											where (e.EmployeeID == empId)
											select e).SingleOrDefault<Employee>();";

            var solGenerator = new RoslynSimpleSolutionGenerator(textToPlaceInMainMethod);

            SemanticModel semanticModelForMainClass =await solGenerator.GetSemanticModelForMainClass();

            target = new LINQToSQLDatabaseQueryExtractor(semanticModelForMainClass, entities);

            //Act
            target.Visit(await solGenerator.GetRootNodeForMainDocument());

            //Assert
            var listResult = target.DatabaseQueries.ToList();
            Assert.IsTrue(listResult.Count == 1);
            Assert.IsTrue(listResult[0].EntityDeclarationsUsedInQuery.Count == 1);
            Assert.IsTrue(listResult[0].EntityDeclarationsUsedInQuery[0].Name == "L2S_Northwind.Employee");
        }

        /// <summary>
        /// ToDo: Check on stackoverflow how to get the text correctly, 
        /// or just write a regex to trim the spaces and lines
        /// </summary>
        [TestMethod]
        [Ignore]
        public async Task ExtractsOneQueryWithCorrectQueryText_When_NoQueryVariableIsDeclared()
        {
            //Arrange
            List<DatabaseEntityDeclaration<LINQToSQL>> entities = new List<DatabaseEntityDeclaration<LINQToSQL>>();
            entities.Add(new DatabaseEntityDeclaration<LINQToSQL>("L2S_Northwind.Employee"));

            string textToPlaceInMainMethod = @" 
									NorthWindDataClassesDataContext dc = new NorthWindDataClassesDataContext();
                                     
									return (from e in dc.Employees
											where (e.EmployeeID == empId)
											select e).SingleOrDefault<Employee>();";

            var solGenerator = new RoslynSimpleSolutionGenerator(textToPlaceInMainMethod);

            SemanticModel semanticModelForMainClass =await solGenerator.GetSemanticModelForMainClass();

            target = new LINQToSQLDatabaseQueryExtractor(semanticModelForMainClass, entities);

            //Act
            target.Visit(await solGenerator.GetRootNodeForMainDocument());

            //Assert
            var listResult = target.DatabaseQueries.ToList();
            Assert.IsTrue(listResult.Count == 1);
            Assert.IsTrue(listResult[0].QueryAsString == @"(from e in dc.Employees
                                            where(e.EmployeeID == empId)
                                            select e)");
        }
    }
}
