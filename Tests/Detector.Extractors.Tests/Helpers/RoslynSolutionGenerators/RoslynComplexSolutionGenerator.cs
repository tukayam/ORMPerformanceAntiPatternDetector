using Microsoft.CodeAnalysis;
using System.Threading.Tasks;

namespace Detector.Extractors.Tests.Helpers.RoslynSolutionGenerators
{
    /// <summary>
    /// Generates Roslyn Solution with expected real life code
    /// </summary>
    public class RoslynComplexSolutionGenerator
    {
        Solution RoslynSolution;
        ProjectId projectId;
        DocumentId DataContextClassId;
        DocumentId OrderEntityDeclarationClassId;
        DocumentId OrderItemEntityDeclarationClassId;
        DocumentId CustomerEntityDeclarationClassId;
        DocumentId MainClassId;
        DocumentId CustomerRepositoryInterfaceId;
        DocumentId CustomerRepositoryClassId;
        DocumentId OrderRepositoryInterfaceId;
        DocumentId OrderRepositoryClassId;

        public RoslynComplexSolutionGenerator()
        {
            projectId = ProjectId.CreateNewId();
            DataContextClassId = DocumentId.CreateNewId(projectId);
            OrderEntityDeclarationClassId = DocumentId.CreateNewId(projectId);
            OrderItemEntityDeclarationClassId = DocumentId.CreateNewId(projectId);
            CustomerEntityDeclarationClassId = DocumentId.CreateNewId(projectId);
            MainClassId = DocumentId.CreateNewId(projectId);
            CustomerRepositoryInterfaceId = DocumentId.CreateNewId(projectId);
            CustomerRepositoryClassId = DocumentId.CreateNewId(projectId);
            OrderRepositoryInterfaceId = DocumentId.CreateNewId(projectId);
            OrderRepositoryClassId = DocumentId.CreateNewId(projectId);

            RoslynSolution = GetRoslynSolution();
        }

        public Task<SemanticModel> GetSemanticModelForMainClass()
        {
            var document = RoslynSolution.GetDocument(MainClassId);
            return document.GetSemanticModelAsync();
        }

        public Task<SyntaxNode> GetRootNodeForMainDocument()
        {
            Document document = RoslynSolution.GetDocument(MainClassId);
            return document.GetSyntaxRootAsync();
        }

        public Task<SyntaxNode> GetRootNodeForCustomerRepositoryClassDocument()
        {
            Document document = RoslynSolution.GetDocument(CustomerRepositoryClassId);
            return document.GetSyntaxRootAsync();
        }

        public Task<SemanticModel> GetSemanticModelForCustomerRepositoryClass()
        {
            var document = RoslynSolution.GetDocument(CustomerRepositoryClassId);
            return document.GetSemanticModelAsync();
        }

        public Solution GetRoslynSolution()
        {
            var solution = new AdhocWorkspace().CurrentSolution
                .AddProject(projectId, "MyProject", "MyProject", LanguageNames.CSharp)
                //.AddMetadataReference(projectId, MetadataReference.CreateFromAssembly(typeof(object).Assembly))
                //.AddMetadataReference(projectId, MetadataReference.CreateFromAssembly(typeof(System.Data.Linq.DataContext).Assembly))
                //.AddMetadataReference(projectId, MetadataReference.CreateFromAssembly(typeof(System.Data.DataTable).Assembly))
				.AddMetadataReference(projectId, MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
				.AddMetadataReference(projectId, MetadataReference.CreateFromFile(typeof(System.Data.Linq.DataContext).Assembly.Location))
				.AddMetadataReference(projectId, MetadataReference.CreateFromFile(typeof(System.Data.DataTable).Assembly.Location))
                .AddDocument(DataContextClassId, "DataContext.cs", GetDataContextCSharpDocumentText())
                .AddDocument(OrderEntityDeclarationClassId, "Order.cs", GetOrderEntityDeclarationClassCSharpDocumentText())
                .AddDocument(CustomerEntityDeclarationClassId, "Customer.cs", GetCustomerEntityDeclarationClassCSharpDocumentText())
                .AddDocument(OrderItemEntityDeclarationClassId, "OrderItem.cs", GetOrderItemEntityDeclarationClassCSharpDocumentText())
                .AddDocument(CustomerRepositoryInterfaceId, "CustomerRepositoryInterface.cs", GetCustomerRepositoryInterfaceCSharpDocumentText())
                .AddDocument(CustomerRepositoryClassId, "CustomerRepository.cs", GetCustomerRepositoryClassCSharpDocumentText())
                .AddDocument(OrderRepositoryInterfaceId, "OrderRepositoryInterface.cs", GetOrderRepositoryInterfaceCSharpDocumentText())
                .AddDocument(OrderRepositoryClassId, "OrderRepository.cs", GetOrderRepositoryClassCSharpDocumentText())
                .AddDocument(MainClassId, "MainClass.cs", GetMainClassCSharpDocumentText());

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

        private string GetOrderEntityDeclarationClassCSharpDocumentText()
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

                                    public int CustomerID;

                                    [global::System.Data.Linq.Mapping.AssociationAttribute]
                                    public EntitySet<OrderItem> OrderItems;
                                }          
                            }";

            return text;
        }

        private string GetOrderItemEntityDeclarationClassCSharpDocumentText()
        {
            string text = @" using System.Data.Linq;
                            using System.Data.Linq.Mapping;
                            using System.Linq;
						    namespace L2S_Northwind
						    {
                                [global::System.Data.Linq.Mapping.TableAttribute]
                                public partial class OrderItem
                                {
                                    [global::System.Data.Linq.Mapping.ColumnAttribute]
                                    public int OrderItemID;

                                    public int OrderID;
                                    public int Price;
                                }
                            }";

            return text;
        }

        private string GetCustomerEntityDeclarationClassCSharpDocumentText()
        {
            string text = @" using System.Data.Linq;
                            using System.Data.Linq.Mapping;
                            using System.Linq;
						    namespace L2S_Northwind
						    {
                                [global::System.Data.Linq.Mapping.TableAttribute]
                                public partial class Customer
                                {
                                     [global::System.Data.Linq.Mapping.ColumnAttribute]
                                     public int CustomerID;

                                     [global::System.Data.Linq.Mapping.AssociationAttribute]
                                     public EntitySet<Order> Orders;
                                }       
                            }";

            return text;
        }

        private string GetCustomerRepositoryInterfaceCSharpDocumentText()
        {
            string text = @" using System.Data.Linq;
                            using System.Data.Linq.Mapping;
                            using System.Linq;
						    namespace L2S_Northwind
						    {
                                public interface CustomerRepository
                                {
                                    Customer GetCustomer(int empId);
                                    IEnumerable<Customer> GetCustomers();
                                } 
                            }";

            return text;
        }

        private string GetCustomerRepositoryClassCSharpDocumentText()
        {
            string text = @" using System.Data.Linq;
                            using System.Data.Linq.Mapping;
                            using System.Linq;
						    namespace L2S_Northwind
						    {
                                public class CustomerRepositoryClass : CustomerRepository
                                {
                                    public Customer GetCustomer(int customerId)
                                    {
                                        NorthWindDataClassesDataContext dc = new NorthWindDataClassesDataContext();
                                        DataLoadOptions options = new DataLoadOptions();
                                        options.LoadWith<Customer>(employee => employee.Orders);
                                        dc.LoadOptions = options;

                                        return (from e in dc.Customers
                                                where (e.CustomerID == customerId)
                                                select e).SingleOrDefault<Customer>();
                                    }

                                    public IEnumerable<Customer> GetCustomers()
                                    {
                                        NorthWindDataClassesDataContext dc = new NorthWindDataClassesDataContext();
                                        return dc.Customers;
                                    }
                                }    
                            }";

            return text;
        }

        private string GetOrderRepositoryInterfaceCSharpDocumentText()
        {
            string text = @" using System.Data.Linq;
                            using System.Data.Linq.Mapping;
                            using System.Linq;
						    namespace L2S_Northwind
						    {
                                 public interface OrderRepository
                                 {
                                    IEnumerable<Order> GetOrdersOfCustomer(int employeeId);
                                 }
                            }";

            return text;
        }

        private string GetOrderRepositoryClassCSharpDocumentText()
        {
            string text = @" using System.Data.Linq;
                            using System.Data.Linq.Mapping;
                            using System.Linq;
						    namespace L2S_Northwind
						    {
                                public class OrderRepositoryClass : OrderRepository
                                {
                                    public IEnumerable<Order> GetOrdersOfCustomer(int customerId)
                                    {
                                        NorthWindDataClassesDataContext dc = new NorthWindDataClassesDataContext();
                                        return (from o in dc.Orders
                                                where o.CustomerID == customerId
                                                select o).ToList();
                                    }
                                }
                            }";

            return text;
        }

        private string GetMainClassCSharpDocumentText()
        {
            string text = @" using System.Data.Linq;
                            using System.Data.Linq.Mapping;
                            using System.Linq;
						    namespace L2S_Northwind
						    {
                                public class ClassUnderTest
                                {
                                    private CustomerRepository _customerRepository;
                                    private OrderRepository _orderRepository;

                                    public ClassUnderTest(CustomerRepository customerRepository, OrderRepository orderRepository)
                                    {
                                        this._customerRepository = customerRepository;
                                        this._orderRepository = orderRepository;
                                    }

                                    public void FindWellPayingCustomers()
                                    {
                                        List<Customer> wellPayingCustomers = new List<Customer>();

                                        foreach (var customer in _customerRepository.GetCustomers())
                                        {
                                            int totalPurchases = 0;
                                            foreach (var order in customer.Orders)
                                            {
                                                foreach (var item in order.OrderItems)
                                                {
                                                    totalPurchases += item.Price;
                                                }
                                            }

                                            if (totalPurchases > 50)
                                            {
                                                wellPayingCustomers.Add(customer);
                                            }
                                        }
                                    }
                                }
                            }";

            return text;
        }
    }
}
