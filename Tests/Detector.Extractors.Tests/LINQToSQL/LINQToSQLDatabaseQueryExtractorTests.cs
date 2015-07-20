using Microsoft.VisualStudio.TestTools.UnitTesting;
using Detector.Models.ORM;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Detector.Extractors.LINQToSQL40;
using Detector.Extractors.Tests.Helpers.RoslynSolutionGenerators;
using Detector.Models.Others;

namespace Detector.Extractors.Tests
{
    [TestClass]
    public class LINQToSQLDatabaseQueryExtractorTests
    {
        //LINQToSQLDatabaseQueryExtractor target;

        //[TestMethod]
        //public async Task ExtractsDatabaseQueryObjectWithAVariable_When_QueryIsInQuerySyntaxAndQueryIsAssignedToAVariable()
        //{
        //    //Arrange
        //    var solGenerator = new RoslynSimpleSolutionGenerator()
        //                   .WithDbAccessingMethodCallOnQueryVariableAndQueryInQuerySyntax();

        //    LINQToSQLDatabaseQueryExtractor target
        //        = await new TargetBuilder().Build(solGenerator);

        //    //Act
        //    target.Visit(await solGenerator.GetRootNodeForMainDocument());
        //    var result = target.DatabaseQueries.First();

        //    //Assert
        //    Assert.IsTrue(target.DatabaseQueries.Count == 1);
        //    Assert.IsTrue(result.DatabaseQueryVariable.VariableName == "query");
        //}

        //[TestMethod]
        //public async Task ExtractsDatabaseQueryObjectWithAVariable_When_QueryIsInMethodSyntaxAndQueryIsAssignedToAVariable()
        //{
        //    //Arrange
        //    var solGenerator = new RoslynSimpleSolutionGenerator()
        //                   .WithDbAccessingMethodCallOnQueryVariableAndQueryInMethodSyntax();

        //    LINQToSQLDatabaseQueryExtractor target
        //        = await new TargetBuilder().Build(solGenerator);

        //    //Act
        //    target.Visit(await solGenerator.GetRootNodeForMainDocument());
        //    var result = target.DatabaseQueries.First();

        //    //Assert
        //    Assert.IsTrue(target.DatabaseQueries.Count == 1);
        //    Assert.IsTrue(result.DatabaseQueryVariable.VariableName == "query");
        //}

        //[TestMethod]
        //public async Task ExtractsOneQueryWithCorrectAmountOfUsedEntities_When_QueryIsInMethodSyntaxAndNoQueryVariableIsDeclared()
        //{
        //    //Arrange
        //    var solGenerator = new RoslynSimpleSolutionGenerator()
        //                   .WithDbAccessingMethodCallOnSameLineAsQueryInMethodSyntax();

        //    LINQToSQLDatabaseQueryExtractor target
        //        = await new TargetBuilder().Build(solGenerator);

        //    //Act
        //    target.Visit(await solGenerator.GetRootNodeForMainDocument());
        //    var result = target.DatabaseQueries;

        //    //Assert
        //    Assert.IsTrue(result.Count == 1);
        //    Assert.IsTrue(result.First().EntityDeclarationsUsedInQuery.Count == 1);
        //    Assert.IsTrue(result.First().EntityDeclarationsUsedInQuery.First().Name == "L2S_Northwind.Employee");
        //}

        ///// <summary>
        ///// ToDo: Check on stackoverflow how to get the text correctly, 
        ///// or just write a regex to trim the spaces and lines
        ///// </summary>
        //[TestMethod]
        //[Ignore]
        //public async Task ExtractsOneQueryWithCorrectQueryText_When_NoQueryVariableIsDeclared()
        //{
        //    //Arrange
        //    var solGenerator = new RoslynSimpleSolutionGenerator()
        //                   .WithDbAccessingMethodCallOnSameLineAsQueryInMethodSyntax();

        //    LINQToSQLDatabaseQueryExtractor target
        //        = await new TargetBuilder().Build(solGenerator);

        //    //Act
        //    target.Visit(await solGenerator.GetRootNodeForMainDocument());
        //    var result = target.DatabaseQueries;

        //    //Assert
        //    Assert.IsTrue(result.Count == 1);
        //    Assert.IsTrue(result.First().QueryTextInCSharp == @"(from e in dc.Employees
        //                                    where(e.EmployeeID == empId)
        //                                    select e)");
        //}

        //public class TargetBuilder
        //{
        //    ModelCollection<DatabaseEntityDeclaration<LINQToSQL>> entities;

        //    public TargetBuilder()
        //    {
        //        entities = new ModelCollection<DatabaseEntityDeclaration<LINQToSQL>>();
        //        entities.Add(new DatabaseEntityDeclaration<LINQToSQL>("L2S_Northwind.Employee"));
        //    }

        //    public async Task<LINQToSQLDatabaseQueryExtractor> Build(RoslynSimpleSolutionGenerator solutionGenerator)
        //    {
        //        SemanticModel semanticModelForMainClass = await solutionGenerator.GetSemanticModelForMainClass();

        //        return new LINQToSQLDatabaseQueryExtractor(semanticModelForMainClass, entities);
        //    }
        //}

    }
}
