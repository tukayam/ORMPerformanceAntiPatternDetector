using Detector.Main.Tests.Helper;
using Detector.Models;
using Detector.Models.ORM.Base;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Detector.Main.Tests
{
    [TestClass]
    public class RoslynSyntaxTreeWalkerTests
    {

        RoslynSyntaxTreeWalker target;

        [TestMethod]
        public void ExtractsTreeWithDataContextInitializationStatement()
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
                            public static Employee GetEmployeeById(int empId)
                            {
                                NorthWindDataClassesDataContext dc = new NorthWindDataClassesDataContext();                                
                            }} ;";
            SyntaxNode rootNode = RoslynSyntaxTreeParser.GetRootSyntaxNodeForText(text);

            target = new RoslynSyntaxTreeWalker();

            //Act
            target.Visit(rootNode);

            //Assert
            Assert.IsTrue(target.ORMSyntaxTree.Nodes[0] is DataContextInitializationStatement);
            Assert.IsTrue(((DataContextInitializationStatement)target.ORMSyntaxTree.Nodes[0]).CompilationUnit.ParentMethodDeclaration.MethodName == "GetEmployeeById");
        }


        [TestMethod]
        public void ExtractsTreeWithAllPossibleObjectsInTheRightOrder()
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
                            #endregion
                           
                            public static Employee GetEmployeeById(int empId)
                            {
                                NorthWindDataClassesDataContext dc = new NorthWindDataClassesDataContext();                                

                                return (from e in dc.GetTable<Employee>()
                                        where (e.EmployeeID == empId)
                                        select e).SingleOrDefault<Employee>();
                            }

                            private static List<string> GetCustomerNames()
                            {
                                List<string> customerNames=new List<string>();
                                var employees= from e in dc.GetTable<Employee>()
                                            select e;

                                foreach(var employee in employees)
                                {
                                    foreach(var order in employee.Order)
                                    {
                                        customerNames.Add(order.CustomerID.ToString());
                                    }
                                }
                                
                                return customerNames;
                            }

                         } ;";
            SyntaxNode rootNode = RoslynSyntaxTreeParser.GetRootSyntaxNodeForText(text);

            target = new RoslynSyntaxTreeWalker();

            //Act
            target.Visit(rootNode);

            //Assert
            Assert.IsTrue(target.ORMSyntaxTree.Nodes[0] is MethodDeclaration);
        }

    }
}
