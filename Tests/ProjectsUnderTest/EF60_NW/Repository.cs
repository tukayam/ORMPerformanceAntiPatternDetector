using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF60_NW
{
    public class Repository : IRepository
    {
        public IQueryable<Customer> GetAllCustomers()
        {
            using (NWDbContext dbContext=new NWDbContext())
            {
                return dbContext.Customers;
            }
        }
    }
}
