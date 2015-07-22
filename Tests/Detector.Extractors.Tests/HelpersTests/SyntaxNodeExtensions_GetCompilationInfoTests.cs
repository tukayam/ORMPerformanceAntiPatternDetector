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
        public async Task SetsSyntaxNodeCorrectly()
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
            CompilationInfo result = dataContextVariableDecSyntaxNode.GetCompilationInfo(null);

            //Assert
            Assert.IsTrue(result.SyntaxNode == dataContextVariableDecSyntaxNode);
        }
    }
}
