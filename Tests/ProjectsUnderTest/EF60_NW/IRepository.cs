using System.Linq;

namespace EF60_NW
{
    public interface IRepository
    {
        IQueryable<Customer> GetAllCustomers();
    }
}
