using Detector.Extractors.DatabaseEntities;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Detector.Extractors.Tests.RoslynSolutionGenerators
{
    public class RoslynSolutionGenerator
    {
        Solution RoslynSolution;
        ProjectId projectId;
        DocumentId DataContextClassDocumentId;
        DocumentId OrderClassDocumentId;
        DocumentId EmployeeClassDocumentId;
        DocumentId MainClassDocumentId;

        public RoslynSolutionGenerator()
            : this(string.Empty)
        { }

        public RoslynSolutionGenerator(string textToPlaceInMethod)
        {
            projectId = ProjectId.CreateNewId();
            DataContextClassDocumentId = DocumentId.CreateNewId(projectId);
            OrderClassDocumentId = DocumentId.CreateNewId(projectId);
            EmployeeClassDocumentId = DocumentId.CreateNewId(projectId);
            MainClassDocumentId = DocumentId.CreateNewId(projectId);

            RoslynSolution = GetRoslynSolution(textToPlaceInMethod);
        }

        public SemanticModel GetSemanticModelForMainClass()
        {
            var document = RoslynSolution.GetDocument(MainClassDocumentId);
            var model = document.GetSemanticModelAsync().Result;

            return model;
        }

        public SyntaxNode GetRootNodeForMainDocument()
        {
            Document document = RoslynSolution.GetDocument(MainClassDocumentId);
            return document.GetSyntaxRootAsync().Result;
        }

        public SyntaxNode GetRootForEntityDocument()
        {
            Document document = RoslynSolution.GetDocument(EmployeeClassDocumentId);
            return document.GetSyntaxRootAsync().Result;
        }

        private Solution GetRoslynSolution(string textToPlaceInMethod)
        {
            var solution = new AdhocWorkspace().CurrentSolution
                .AddProject(projectId, "MyProject", "MyProject", LanguageNames.CSharp)
                .AddMetadataReference(projectId, MetadataReference.CreateFromAssembly(typeof(object).Assembly))
                .AddMetadataReference(projectId, MetadataReference.CreateFromAssembly(typeof(System.Data.Linq.DataContext).Assembly))
                .AddMetadataReference(projectId, MetadataReference.CreateFromAssembly(typeof(System.Data.DataTable).Assembly))
                .AddDocument(DataContextClassDocumentId, "DataContext.cs", GetDataContextCSharpDocumentText())
                .AddDocument(OrderClassDocumentId, "Order.cs", GetOrderClassCSharpDocumentText())
                .AddDocument(EmployeeClassDocumentId, "Employee.cs", GetEmployeeClassCSharpDocumentText())
                .AddDocument(MainClassDocumentId, "MainClass.cs", GetMainClassCSharpDocumentText(textToPlaceInMethod));

            return solution;
        }

        private string GetDataContextCSharpDocumentText()
        {
            string text = @" using System.Data.Linq;
                            using System.Data.Linq.Mapping;
                            using System.Linq;
						namespace L2S_Northwind
						{
                            [global::System.Data.Linq.Mapping.DatabaseAttribute]
                            public partial class NorthWindDataClassesDataContext : System.Data.Linq.DataContext
                            {
                                   partial void OnCreated();
                                    private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
                                    public NorthWindDataClassesDataContext() :
                                        base("", mappingSource)
                                    {
                                        OnCreated();
                                    }

                                    public System.Data.Linq.Table<Employee> Employees
                                    {
                                        get
                                        {
                                            return this.GetTable<Employee>();
                                        }
                                    }

                                    public System.Data.Linq.Table<Order> Orders
                                    {
                                        get
                                        {
                                            return this.GetTable<Order>();
                                        }
                                    }
                            }                            
                        }";

            return text;
        }

        private string GetOrderClassCSharpDocumentText()
        {
            string text = @" using System.Data.Linq;
                            using System.Data.Linq.Mapping;
                            using System.Linq;
						namespace L2S_Northwind
						{
                            [global::System.Data.Linq.Mapping.TableAttribute]
                            public partial class Order
                            {
                                [global::System.Data.Linq.Mapping.ColumnAttribute]
                                public int OrderID;
                            }          
                        }";

            return text;
        }

        private string GetEmployeeClassCSharpDocumentText()
        {
            string text = @" using System.Data.Linq;
                            using System.Data.Linq.Mapping;
                            using System.Linq;
						namespace L2S_Northwind
						{
                            [global::System.Data.Linq.Mapping.TableAttribute]
                            public partial class Employee
                            {
                                 [global::System.Data.Linq.Mapping.ColumnAttribute]
                                 public int EmployeeID;

                                 [global::System.Data.Linq.Mapping.AssociationAttribute]
                                 public EntitySet<Order> Orders;
                            }       
                        }";

            return text;
        }

        private string GetMainClassCSharpDocumentText(string textToPlaceInMethod)
        {
            string text = @" using System.Data.Linq;
                            using System.Data.Linq.Mapping;
                            using System.Linq;
						namespace L2S_Northwind
						{
                            public class ClassUnderTest
							{
								public static Employee GetEmployeeById(int empId)
								{
									" + textToPlaceInMethod + @"
								}
							}    
                        }";

            return text;
        }
    }
}
