using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Entities.Orders
{
    public enum OrderStatus
    {
        Pending = 0,
        PaymentSuccess = 1,
        PaymentFailed = 2,
    }
}
