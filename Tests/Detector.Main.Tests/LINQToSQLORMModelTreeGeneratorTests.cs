using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Detector.Extractors.Tests
{
    [TestClass]
    public class LINQToSQLORMModelTreeGeneratorTests
    {
        LINQToSQLORMModelTreeGenerator target;
        //DatabaseEntityDeclarationExtractor<LINQToSQL> _databaseEntityDeclarationsExtractor;
        //[TestInitialize]
        //public void Initialize()
        //{
        //    var entityDeclarations = new List<DatabaseEntityDeclaration<LINQToSQL>>();
        //    entityDeclarations.Add(new DatabaseEntityDeclaration<LINQToSQL>("L2S_Northwind.Employee"));
        //    entityDeclarations.Add(new DatabaseEntityDeclaration<LINQToSQL>("L2S_Northwind.Order"));

        //    var mock = new Mock<DatabaseEntityDeclarationExtractor<LINQToSQL>>();
        //    mock.Setup(foo => foo.DatabaseEntityDeclarations).Returns(entityDeclarations);
        //    _databaseEntityDeclarationsExtractor = mock.Object;
        //}

        //[TestMethod]
        //public void CreatesCorrectORMModelTree_When_DocumentHasDatabaseAccessingMethodOnAQueryLine()
        //{
        //    //Arrange
        //    string textToPlaceInMainMethod = @" 
								//	var dc = new NorthWindDataClassesDataContext();								
									
								//	return (from e in dc.Employees
								//			where (e.EmployeeID == empId)
								//			select e).SingleOrDefault<Employee>();";

        //    var solGenerator = new RoslynSolutionGenerator(textToPlaceInMainMethod);
        //    SemanticModel semanticModelForMainClass = solGenerator.GetSemanticModelForMainClass();
        //    target = new LINQToSQLORMModelTreeExtractor(semanticModelForMainClass, _databaseEntityDeclarationsExtractor);

        //    //Act
        //    target.Visit(solGenerator.GetRootNodeForMainDocument());

        //    ORMModelTree result = target.GetORMModelTree();

        //    MethodDeclaration root = result.RootNode as MethodDeclaration;
        //    var children = result.RootNode.ChildNodes.ToList();

        //    //Assert
        //    Assert.IsTrue(root.MethodName == "GetEmployeeById");
        //    Assert.IsTrue(children[0] is DataContextDeclaration<LINQToSQL>);
        //    Assert.IsTrue(children[1] is DatabaseAccessingMethodCallStatementOnQueryDeclaration<LINQToSQL>);
        //}
    }
}
