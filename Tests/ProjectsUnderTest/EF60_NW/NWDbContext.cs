using System.Data.Entity;

namespace EF60_NW
{
    public class NWDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
