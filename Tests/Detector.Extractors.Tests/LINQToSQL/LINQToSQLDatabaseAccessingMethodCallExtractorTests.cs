using Microsoft.VisualStudio.TestTools.UnitTesting;
using Detector.Models.ORM;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Detector.Extractors.LINQToSQL40;
using Detector.Extractors.Tests.Helpers.RoslynSolutionGenerators;
using Detector.Models.Others;

namespace Detector.Extractors.Tests
{
    [TestClass]
    public class LINQToSQLDatabaseAccessingMethodCallExtractorTests
    {
        [TestMethod]
        public async Task DetectsDatabaseAccessingMethodCall_When_DBAccessingMethodIsOnSameSentenceAsQuery()
        {
            //Arrange
            var solGenerator = new RoslynSimpleSolutionGenerator()
                           .WithDbAccessingMethodCallOnSameLineAsQueryInQuerySyntax();

            LINQToSQLDatabaseAccessingMethodCallExtractor target
                = await new TargetBuilder()
                .BuildWithDatabaseQueryWithNoVariable(solGenerator);

            //Act
            target.Visit(await solGenerator.GetRootNodeForMainDocument());

            HashSet<DatabaseAccessingMethodCallStatement<LINQToSQL>> result = target.DatabaseAccessingMethodCalls;

            //Assert
            Assert.IsTrue(result.Count == 1);
        }

        [TestMethod]
        public async Task DetectsDatabaseAccessingMethodCallWithCorrectDatabaseEntities_When_DBAccessingMethodIsOnSameSentenceAsQuery()
        {
            //Arrange
            var solGenerator = new RoslynSimpleSolutionGenerator()
                           .WithDbAccessingMethodCallOnSameLineAsQueryInQuerySyntax();

            LINQToSQLDatabaseAccessingMethodCallExtractor target
                = await new TargetBuilder()
                .BuildWithDatabaseQueryWithNoVariable(solGenerator);

            //Act
            target.Visit(await solGenerator.GetRootNodeForMainDocument());
            DatabaseAccessingMethodCallStatement<LINQToSQL> result = target.DatabaseAccessingMethodCalls.First();

            //Assert
            Assert.IsTrue(result.DatabaseQuery.EntityDeclarationsUsedInQuery.Count == 1);
            Assert.IsTrue(result.DatabaseQuery.EntityDeclarationsUsedInQuery.First().Name == "L2S_Northwind.Employee");
        }

        [TestMethod]
        public async Task DetectsDatabaseAccessingMethodCall_When_QueryIsWrittenInMethodSyntax()
        {
            //Arrange
            var solGenerator = new RoslynSimpleSolutionGenerator()
                           .WithDbAccessingMethodCallOnSameLineAsQueryInMethodSyntax();

            LINQToSQLDatabaseAccessingMethodCallExtractor target
                = await new TargetBuilder()
                .BuildWithDatabaseQueryWithNoVariable(solGenerator);

            //Act
            target.Visit(await solGenerator.GetRootNodeForMainDocument());
            HashSet<DatabaseAccessingMethodCallStatement<LINQToSQL>> result = target.DatabaseAccessingMethodCalls;

            //Assert
            Assert.IsTrue(result.Count == 1);
        }

        [TestMethod]
        public async Task DetectsDatabaseAccessingMethodCall_When_DBAccessingMethodIsOnQueryVariable()
        {
            //Arrange
            var solGenerator = new RoslynSimpleSolutionGenerator()
                           .WithDbAccessingMethodCallOnQueryVariableAndQueryInQuerySyntax();

            LINQToSQLDatabaseAccessingMethodCallExtractor target
                = await new TargetBuilder()
                .BuildWithDatabaseQueryWithVariable(solGenerator);

            //Act
            target.Visit(await solGenerator.GetRootNodeForMainDocument());
            HashSet<DatabaseAccessingMethodCallStatement<LINQToSQL>> result = target.DatabaseAccessingMethodCalls;

            //Assert
            Assert.IsTrue(result.Count == 1);
        }

        [TestMethod]
        public async Task ExtractsDbAccessingMethodCallWithCorrectValues_When_MethodHasALazyLoadingDbAccessingMethodCall()
        {
            //Arrange
            var solGenerator = new RoslynSimpleSolutionGenerator()
                           .WithDbAccessingMethodCallOnSameLineAsQueryInQuerySyntax();

            LINQToSQLDatabaseAccessingMethodCallExtractor target
                = await new TargetBuilder()
                .BuildWithDatabaseQueryWithNoVariable(solGenerator);

            //Act
            target.Visit(await solGenerator.GetRootNodeForMainDocument());
            DatabaseAccessingMethodCallStatement<LINQToSQL> result = target.DatabaseAccessingMethodCalls.First();

            //Assert3
            Assert.IsNull(result.AssignedVariable);
            Assert.IsTrue(result.DatabaseQuery.EntityDeclarationsUsedInQuery.First().Name == "Employee");
            Assert.IsTrue(result.DatabaseQuery.DatabaseQueryVariable.VariableName == "query");
            Assert.IsFalse(result.DoesEagerLoad);
            Assert.IsTrue(result.LoadedEntityDeclarations.Count() == 1);
            Assert.IsTrue(result.LoadedEntityDeclarations[0].Name == "Employee");
        }

        [TestMethod]
        public async Task ExecutedQueryIsSQLQueryWithAJoin_When_DBAccessingMethodQueryReturnsEagerLoadedEntities()
        {
            //Arrange
            var solGenerator = new RoslynSimpleSolutionGenerator()
                           .WithDbAccessingMethodCallOnSameLineAsQueryInQuerySyntax();

            LINQToSQLDatabaseAccessingMethodCallExtractor target
                = await new TargetBuilder()
                .BuildWithDatabaseQueryWithNoVariable(solGenerator);

            //Act
            target.Visit(await solGenerator.GetRootNodeForMainDocument());
            DatabaseAccessingMethodCallStatement<LINQToSQL> result = target.DatabaseAccessingMethodCalls.First();

            //Assert
            Assert.IsTrue(result.ExecutedQuery.ToLower().Contains("join"));
        }

        public class TargetBuilder
        {
            LINQToSQLDatabaseAccessingMethodCallExtractor target;
            ModelCollection<DatabaseEntityDeclaration<LINQToSQL>> _entityDeclarations;

            public TargetBuilder()
            {
                _entityDeclarations = new ModelCollection<DatabaseEntityDeclaration<LINQToSQL>>()
                                        {
                                            new DatabaseEntityDeclaration<LINQToSQL>("L2S_Northwind.Employee")
                                        };
            }

            public async Task<LINQToSQLDatabaseAccessingMethodCallExtractor> BuildWithDatabaseQueryWithNoVariable(RoslynSimpleSolutionGenerator solGenerator)
            {
                SemanticModel semanticModelForMainClass = await solGenerator.GetSemanticModelForMainClass();

                var databaseQueries = new ModelCollection<DatabaseQuery<LINQToSQL>>();
                databaseQueries.Add(new DatabaseQuery<LINQToSQL>(@"from e in dc.Employees
											where (e.EmployeeID == empId)
											select e", _entityDeclarations, null));

                return new LINQToSQLDatabaseAccessingMethodCallExtractor(semanticModelForMainClass, _entityDeclarations, databaseQueries, null, null, null);
            }

            public async Task<LINQToSQLDatabaseAccessingMethodCallExtractor> BuildWithDatabaseQueryWithVariable(RoslynSimpleSolutionGenerator solGenerator)
            {
                SemanticModel semanticModelForMainClass = await solGenerator.GetSemanticModelForMainClass();

                DatabaseQueryVariable dbQV = new DatabaseQueryVariable("query");

                var databaseQueries = new ModelCollection<DatabaseQuery<LINQToSQL>>();
                databaseQueries.Add(new DatabaseQuery<LINQToSQL>(@"from e in dc.Employees
											where (e.EmployeeID == empId)
											select e", _entityDeclarations, dbQV));

                return new LINQToSQLDatabaseAccessingMethodCallExtractor(semanticModelForMainClass, _entityDeclarations, databaseQueries, null, null, null);
            }
        }

    }
}
