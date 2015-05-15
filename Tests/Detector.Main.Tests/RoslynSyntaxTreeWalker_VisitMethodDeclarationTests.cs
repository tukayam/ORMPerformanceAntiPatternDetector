using Detector.Main.Tests.Helper;
using Detector.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Detector.Main.Tests
{
    [TestClass]
    public class RoslynSyntaxTreeWalker_VisitMethodDeclarationTests
    {
        RoslynSyntaxTreeWalker target;

        [TestMethod]
        public void ExtractsTreeListWithMethodDeclarations_When_CodeHasMethod()
        {
            //Arrange
            string text = "public void Method2(){ //whatever }";
            SyntaxNode rootNode = RoslynSyntaxTreeParser.GetRootSyntaxNodeForText(text);

            target = new RoslynSyntaxTreeWalker();

            //Act
            target.Visit(rootNode);

            //Assert
            Assert.IsTrue(target.ORMSyntaxTree.Nodes[0] is MethodDeclaration);
        }
    }
}
