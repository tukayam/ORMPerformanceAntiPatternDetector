using System.Collections.Generic;

namespace EF60_NW
{
    public class Order
    {
        public int OrderID;
        public int CustomerID;
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }

    public partial class OrderItem
    {
        public int OrderItemID;

        public int OrderID;
        public int Price;
    }

    public partial class Customer
    {
        public int CustomerID;
        public virtual ICollection<Order> Orders { get; set; }
    }
}
