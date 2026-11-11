using Store.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Specifications.Orders
{
    internal class OrdersWithIdAndEmailSpec : BaseSpecification<Order, Guid>
    {
        public OrdersWithIdAndEmailSpec(string email) : base(o => o.BuyerEmail == email)
        {
            AddIncludes(x => x.DeliveryMethod);
            AddIncludes(x => x.OrderItems);
            AddOrderByDescending(x => x.OrderDate);
        }

        public OrdersWithIdAndEmailSpec(Guid id, string email) : base(o => o.BuyerEmail == email && o.Id == id)
        {
            AddIncludes(x => x.DeliveryMethod);
            AddIncludes(x => x.OrderItems);
        }
    }
}
