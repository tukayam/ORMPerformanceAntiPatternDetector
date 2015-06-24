using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using Detector.Models.Base;
using Detector.Extractors.Base.Helpers;
using Microsoft.CodeAnalysis;
using System.Threading.Tasks;
using Detector.Extractors.Tests.Helpers.RoslynSolutionGenerators;

namespace Detector.Extractors.Tests.HelpersTests
{
    /// <summary>
    /// Summary description for SyntaxNodeExtensions_GetCompilationInfoTests
    /// </summary>
    [TestClass]
    public class SyntaxNodeExtensions_GetCompilationInfoTests
    {
        [TestMethod]
        public async Task SetsParentMethodDeclarationCorrectly_When_NodeIsInsideAMethod()
        {
            //Arrange
            string textToPlaceInMainMethod = @" 
									NorthWindDataClassesDataContext dc = new NorthWindDataClassesDataContext();
                                    var query = (from e in dc.Employees
											where (e.EmployeeID == empId)
											select e);
									return query.SingleOrDefault<Employee>();";

            var solGenerator = new RoslynSimpleSolutionGenerator(textToPlaceInMainMethod);
            SyntaxNode rootNode = await solGenerator.GetRootNodeForMainDocument();
            var dataContextVariableDecSyntaxNode= rootNode.DescendantNodes().OfType<VariableDeclarationSyntax>().First();

            //Act
            CompilationInfo result = dataContextVariableDecSyntaxNode.GetCompilationInfo();

            //Assert
            Assert.IsTrue(result.ParentMethodDeclaration.MethodName == "GetEmployeeById");
        }

        [TestMethod]
        public async Task SetsParentMethodDeclarationToNull_When_NodeIsNotInsideAMethod()
        {
            //Arrange
            var solGenerator = new RoslynSimpleSolutionGenerator(string.Empty);
            SyntaxNode rootNode = await solGenerator.GetRootNodeForMainDocument();
            ClassDeclarationSyntax dataContextVariableDecSyntaxNode = rootNode.DescendantNodes().OfType<ClassDeclarationSyntax>().First();

            //Act
            CompilationInfo result = dataContextVariableDecSyntaxNode.GetCompilationInfo();

            //Assert
            Assert.IsNull(result.ParentMethodDeclaration);
        }

        [TestMethod]
        public async Task SetsSpanStartCorrectly()
        {
            //Arrange
            string textToPlaceInMainMethod = @" 
									NorthWindDataClassesDataContext dc = new NorthWindDataClassesDataContext();
                                    var query = (from e in dc.Employees
											where (e.EmployeeID == empId)
											select e);
									return query.SingleOrDefault<Employee>();";

            var solGenerator = new RoslynSimpleSolutionGenerator(textToPlaceInMainMethod);
            SyntaxNode rootNode = await solGenerator.GetRootNodeForMainDocument();
            VariableDeclarationSyntax dataContextVariableDecSyntaxNode = rootNode.DescendantNodes().OfType<VariableDeclarationSyntax>().First();

            //Act
            CompilationInfo result = dataContextVariableDecSyntaxNode.GetCompilationInfo();

            //Assert
            Assert.IsTrue(result.SpanStart == 333);
        }
    }
}
