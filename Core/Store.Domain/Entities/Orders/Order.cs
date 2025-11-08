using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Entities.Orders
{
    // Table
    public class Order : BaseEntity<Guid>
    {
        public string UserEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = 
    }
}
