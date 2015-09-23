using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;

namespace L2S_Northwind
{
    #region DataContext
    [Database]
    public partial class NorthWindDataClassesDataContext : DataContext
    {
        partial void OnCreated();
        private static MappingSource mappingSource = new AttributeMappingSource();
        public NorthWindDataClassesDataContext() :
            base("", mappingSource)
        {
            OnCreated();
        }

        public Table<Customer> Customers
        {
            get
            {
                return this.GetTable<Customer>();
            }
        }

        public Table<Order> Orders
        {
            get
            {
                return this.GetTable<Order>();
            }
        }
    }
    #endregion

    #region Models

    [Table]
    public partial class Order
    {
        [Column(DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true)]
        public int OrderID;

        public int CustomerID;

        [Association(Name = "Order_OrderItem", ThisKey = "OrderID", OtherKey = "OrderID")]
        public EntitySet<OrderItem> OrderItems;

    }

    [Table]
    public partial class OrderItem
    {
        [Column(DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true)]
        public int OrderItemID;

        public int OrderID;
        public int Price;
    }

    [Table]
    public partial class Customer
    {
        [Column(DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true)]
        public int CustomerID;

        [Association(Name = "Customer_Order", ThisKey = "CustomerID", OtherKey = "CustomerID")]
        public EntitySet<Order> Orders;
    }

    #endregion

    #region Repositories

    public interface CustomerRepository
    {
        Customer GetCustomer(int empId);
        IEnumerable<Customer> GetCustomers();
    }

    public interface OrderRepository
    {
        IEnumerable<Order> GetOrdersOfCustomer(int employeeId);
    }

    public class ConcreteCustomerRepository : CustomerRepository
    {
        public Customer GetCustomer(int customerId)
        {
            using (NorthWindDataClassesDataContext dc = new NorthWindDataClassesDataContext())
            {
                DataLoadOptions options = new DataLoadOptions();
                options.LoadWith<Customer>(c => c.Orders);
                dc.LoadOptions = options;

                return (from e in dc.Customers
                        where (e.CustomerID == customerId)
                        select e).SingleOrDefault();
            }
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
            using (NorthWindDataClassesDataContext dc = new NorthWindDataClassesDataContext())
            {
                return (from o in dc.Orders
                        where o.CustomerID == customerId
                        select o).ToList();
            }
        }
    }
    #endregion

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
}