﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Detector.Models.ORM;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Detector.Extractors.Tests.RoslynSolutionGenerators;
using System.Threading.Tasks;

namespace Detector.Extractors.Tests
{
    [TestClass]
    public class LINQToSQLDatabaseAccessingMethodCallExtractorTests
    {
        LINQToSQLDatabaseAccessingMethodCallExtractor target;

        List<DatabaseEntityDeclaration<LINQToSQL>> _entityDeclarations;

        [TestInitialize]
        public void Initialize()
        {
            _entityDeclarations = new List<DatabaseEntityDeclaration<LINQToSQL>>()
                                        {
                                            new DatabaseEntityDeclaration<LINQToSQL>("L2S_Northwind.Employee")
                                        };
        }

        [TestMethod]
        public async Task DetectsDatabaseAccessingMethodCall_When_DBAccessingMethodIsOnSameSentenceAsQuery()
        {
            //Arrange
            string textToPlaceInMainMethod = @" 
									var dc = new NorthWindDataClassesDataContext();								
									
									return (from e in dc.Employees
											where (e.EmployeeID == empId)
											select e).SingleOrDefault<Employee>();";

            var solGenerator = new RoslynSimpleSolutionGenerator(textToPlaceInMainMethod);

            SemanticModel semanticModelForMainClass =await solGenerator.GetSemanticModelForMainClass();

            var databaseQueries = new List<DatabaseQuery<LINQToSQL>>();
            databaseQueries.Add(new DatabaseQuery<LINQToSQL>(@"from e in dc.Employees
											where (e.EmployeeID == empId)
											select e", _entityDeclarations));

            target = new LINQToSQLDatabaseAccessingMethodCallExtractor(semanticModelForMainClass, _entityDeclarations, databaseQueries);

            //Act
            target.Visit(await solGenerator.GetRootNodeForMainDocument());

            List<DatabaseAccessingMethodCallStatement<LINQToSQL>> result = target.DatabaseAccessingMethodCalls;

            //Assert
            Assert.IsTrue(result.Count == 1);
        }

        [TestMethod]
        public async Task DetectsDatabaseAccessingMethodCallWithCorrectDatabaseEntities_When_DBAccessingMethodIsOnSameSentenceAsQuery()
        {
            //Arrange
            string textToPlaceInMainMethod = @" 
									var dc = new NorthWindDataClassesDataContext();								
									
									return (from e in dc.Employees
											where (e.EmployeeID == empId)
											select e).SingleOrDefault<Employee>();";

            var solGenerator = new RoslynSimpleSolutionGenerator(textToPlaceInMainMethod);

            SemanticModel semanticModelForMainClass =await solGenerator.GetSemanticModelForMainClass();

            var databaseQueries = new List<DatabaseQuery<LINQToSQL>>();
            databaseQueries.Add(new DatabaseQuery<LINQToSQL>(@"from e in dc.Employees
											where (e.EmployeeID == empId)
											select e", _entityDeclarations));

            target = new LINQToSQLDatabaseAccessingMethodCallExtractor(semanticModelForMainClass, _entityDeclarations, databaseQueries);

            //Act
            target.Visit(await solGenerator.GetRootNodeForMainDocument());

            List<DatabaseAccessingMethodCallStatement<LINQToSQL>> result = target.DatabaseAccessingMethodCalls;

            //Assert
            Assert.IsTrue(result[0].DatabaseQuery.EntityDeclarationsUsedInQuery.Count == 1);
            Assert.IsTrue(result[0].DatabaseQuery.EntityDeclarationsUsedInQuery[0].Name == "L2S_Northwind.Employee");
        }

        [TestMethod]
        public async Task DetectsDatabaseAccessingMethodCall_When_DBAccessingMethodIsOnQueryVariable()
        {
            //Arrange
            string textToPlaceInMainMethod = @" 
									NorthWindDataClassesDataContext dc = new NorthWindDataClassesDataContext();
                                    var query = (from e in dc.Employees
											where (e.EmployeeID == empId)
											select e);
									return query.SingleOrDefault<Employee>();";

            var solGenerator = new RoslynSimpleSolutionGenerator(textToPlaceInMainMethod);

            SemanticModel semanticModelForMainClass = await solGenerator.GetSemanticModelForMainClass();
            var databaseQueries = new List<DatabaseQuery<LINQToSQL>>();
            databaseQueries.Add(new DatabaseQuery<LINQToSQL>(@"from e in dc.Employees
											where (e.EmployeeID == empId)
											select e", _entityDeclarations, new DatabaseQueryVariable("query")));

            target = new LINQToSQLDatabaseAccessingMethodCallExtractor(semanticModelForMainClass, _entityDeclarations, databaseQueries);

            //Act
            target.Visit(await solGenerator.GetRootNodeForMainDocument());
            List<DatabaseAccessingMethodCallStatement<LINQToSQL>> result = target.DatabaseAccessingMethodCalls;

            //Assert
            Assert.IsTrue(result.Count == 1);
        }
    }
}