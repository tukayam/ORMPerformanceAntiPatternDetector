using System.Linq;

namespace EF60_NW
{
    public interface IRepository
    {
        IQueryable<Customer> GetAllCustomers();
        Customer GetCustomerUsingMethodSyntax(int id);
        Customer GetCustomerUsingMethodSyntaxAndAssignToVariable(int id);
    }
}
