using Detector.Main.Tests.Helper;
using Detector.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Detector.Main.Tests
{
    [TestClass]
    public class RoslynSyntaxTreeWalker_VisitConstructorDeclarationTests
    {

        RoslynSyntaxTreeWalker target;

        [TestMethod]
        public void ExtractsTreeListWithMethodDeclarations_When_CodeHasConstructor()
        {
            //Arrange
            string text =
                @"namespace L2S_Northwind{
                    /// <summary>
                    /// This class is used to demonstrate each of the
                    /// queries defined in the accessor class
                    /// </summary>
                        public partial class frmMain : Form
                        {
                            // used to support take/skip example
                            private int OrderPosition;
                            #region Constructor

                            public frmMain()
                            {
                                InitializeComponent();

                                // set order position to zero
                                OrderPosition = 0;
                            }
                #endregion";
            SyntaxNode rootNode = RoslynSyntaxTreeParser.GetRootSyntaxNodeForText(text);

            target = new RoslynSyntaxTreeWalker();

            //Act
            target.Visit(rootNode);

            //Assert
            Assert.IsTrue(target.ORMSyntaxTree.Nodes[0] is MethodDeclaration);
        }

    }
}
