using System.Data.Linq;
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

    [global::System.Data.Linq.Mapping.TableAttribute]
    public partial class Order
    {
        [global::System.Data.Linq.Mapping.ColumnAttribute(DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true)]
        public int OrderID;
    }

    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.Employees")]
    public partial class Employee
    {
        [global::System.Data.Linq.Mapping.ColumnAttribute(DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true)]
        public int EmployeeID;

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "Employee_Order", ThisKey = "EmployeeID", OtherKey = "EmployeeID")]
        public EntitySet<Order> Orders;
    }
    public class ClassUnderTest
    {
        public static Employee GetEmployeeById(int empId)
        {
            // Eagerly loaded Employee entity (Orders loaded eager)
            NorthWindDataClassesDataContext dc = new NorthWindDataClassesDataContext();
            DataLoadOptions options = new DataLoadOptions();
            options.LoadWith<Employee>(employee => employee.Orders);
            dc.LoadOptions = options;

            //Orders not used
            //var employee = ;
            return (from e in dc.Employees
                    where (e.EmployeeID == empId)
                    select e).SingleOrDefault<Employee>();
        }
    }


}