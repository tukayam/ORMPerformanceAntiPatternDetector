using System.Linq;

namespace EF60_NW
{
    public class Repository : IRepository
    {
        public Customer GetCustomerUsingQuerySyntaxAndAssignToVariable(int id)
        {
            using (NWDbContext dbContext = new NWDbContext())
            {
                var query = (from c in dbContext.Customers
                             where c.CustomerID == id
                             select c);
                return query.First();
            }
        }

        public Customer GetCustomerUsingQuerySyntax(int id)
        {
            using (NWDbContext dbContext = new NWDbContext())
            {
                return (from c in dbContext.Customers
                        where c.CustomerID == id
                        select c).First();
            }
        }

        public Customer GetCustomerUsingMethodSyntax(int id)
        {
            using (NWDbContext dbContext = new NWDbContext())
            {
                return dbContext.Customers.Where(c => c.CustomerID == id).First();
            }
        }

        public Customer GetCustomerUsingMethodSyntaxAndAssignToVariable(int id)
        {
            using (NWDbContext dbContext = new NWDbContext())
            {
                var query = dbContext.Customers.Where(c => c.CustomerID == id).First();
                return query;
            }
        }

        public Customer GetCustomerUsingMethodSyntaxAndQueryIsChangedInMultipleLines(int id)
        {
            using (NWDbContext dbContext = new NWDbContext())
            {
                var query = dbContext.Customers.AsQueryable();
                query = query.Where(c => c.CustomerID == id);
                return query.First();
            }
        }

        public IQueryable<Customer> GetAllCustomers()
        {
            using (NWDbContext dbContext = new NWDbContext())
            {
                return dbContext.Customers;
            }
        }
    }
}

