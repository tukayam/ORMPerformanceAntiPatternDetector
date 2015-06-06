using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Detector.Extractors.Tests.RoslynSolutionGenerators;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using Detector.Models.Base;
using Detector.Extractors.Helpers;

namespace Detector.Extractors.Tests.HelpersTests
{
    /// <summary>
    /// Summary description for SyntaxNodeExtensions_GetCompilationInfoTests
    /// </summary>
    [TestClass]
    public class SyntaxNodeExtensions_GetCompilationInfoTests
    {
        [TestMethod]
        public void SetsParentMethodDeclarationCorrectly_When_NodeIsInsideAMethod()
        {
            //Arrange
            string textToPlaceInMainMethod = @" 
									NorthWindDataClassesDataContext dc = new NorthWindDataClassesDataContext();
                                    var query = (from e in dc.Employees
											where (e.EmployeeID == empId)
											select e);
									return query.SingleOrDefault<Employee>();";

            var solGenerator = new RoslynSolutionGenerator(textToPlaceInMainMethod);
            VariableDeclarationSyntax dataContextVariableDecSyntaxNode = solGenerator.GetRootNodeForMainDocument().DescendantNodes().OfType<VariableDeclarationSyntax>().First();

            //Act
            CompilationInfo result = dataContextVariableDecSyntaxNode.GetCompilationInfo();

            //Assert
            Assert.IsTrue(result.ParentMethodDeclaration.MethodName == "GetEmployeeById");
        }

        [TestMethod]
        public void SetsParentMethodDeclarationToNull_When_NodeIsNotInsideAMethod()
        {
            //Arrange
            var solGenerator = new RoslynSolutionGenerator(string.Empty);
            ClassDeclarationSyntax dataContextVariableDecSyntaxNode = solGenerator.GetRootNodeForMainDocument().DescendantNodes().OfType<ClassDeclarationSyntax>().First();

            //Act
            CompilationInfo result = dataContextVariableDecSyntaxNode.GetCompilationInfo();

            //Assert
            Assert.IsNull(result.ParentMethodDeclaration);
        }

        [TestMethod]
        public void SetsSpanStartCorrectly()
        {
            //Arrange
            string textToPlaceInMainMethod = @" 
									NorthWindDataClassesDataContext dc = new NorthWindDataClassesDataContext();
                                    var query = (from e in dc.Employees
											where (e.EmployeeID == empId)
											select e);
									return query.SingleOrDefault<Employee>();";

            var solGenerator = new RoslynSolutionGenerator(textToPlaceInMainMethod);
            VariableDeclarationSyntax dataContextVariableDecSyntaxNode = solGenerator.GetRootNodeForMainDocument().DescendantNodes().OfType<VariableDeclarationSyntax>().First();

            //Act
            CompilationInfo result = dataContextVariableDecSyntaxNode.GetCompilationInfo();

            //Assert
            Assert.IsTrue(result.SpanStart == 333);
        }
    }
}
