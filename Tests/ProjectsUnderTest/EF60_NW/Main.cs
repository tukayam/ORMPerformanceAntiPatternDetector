using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF60_NW
{
    public class Main
    {
        private IRepository _repository;
        public Main(IRepository repository)
        {
            _repository = repository;
        }

        public void DoSomething()
        {
            var customers = _repository.GetAllCustomers();
            foreach (var customer in customers)
            {
                var purchasesByOrder = from o in customer.Orders
                                     select o.OrderItems.Sum(x => x.Price);

                int totalPurchased = 0;
                foreach (var item in purchasesByOrder)
                {
                    totalPurchased += item;
                }
            }
        }
    }
}
