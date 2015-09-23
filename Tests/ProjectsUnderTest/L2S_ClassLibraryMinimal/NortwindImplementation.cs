using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;

namespace L2S_Northwind
{
    [DatabaseAttribute]
    public partial class NorthWindDataClassesDataContext : System.Data.Linq.DataContext
    {
        partial void OnCreated();
        private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
        public NorthWindDataClassesDataContext() :
            base("", mappingSource)
        {
            OnCreated();
        }

        public System.Data.Linq.Table<Customer> Customers
        {
            get
            {
                return this.GetTable<Customer>();
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

    [TableAttribute]
    public partial class Order
    {
        [global::System.Data.Linq.Mapping.ColumnAttribute(DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true)]
        public int OrderID;

        public int CustomerID;

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "Order_OrderItem", ThisKey = "OrderID", OtherKey = "OrderID")]
        public EntitySet<OrderItem> OrderItems;

    }

    [TableAttribute]
    public partial class OrderItem
    {
        [global::System.Data.Linq.Mapping.ColumnAttribute(DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true)]
        public int OrderItemID;

        public int OrderID;
        public int Price;
    }

    [TableAttribute(Name = "dbo.Employees")]
    public partial class Customer
    {
        [global::System.Data.Linq.Mapping.ColumnAttribute(DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true)]
        public int CustomerID;

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "Employee_Order", ThisKey = "EmployeeID", OtherKey = "EmployeeID")]
        public EntitySet<Order> Orders;
    }

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

    public interface CustomerRepository
    {
        Customer GetCustomer(int empId);
        IEnumerable<Customer> GetCustomers();
    }


    public interface OrderRepository
    {
        IEnumerable<Order> GetOrdersOfCustomer(int employeeId);
    }

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
}