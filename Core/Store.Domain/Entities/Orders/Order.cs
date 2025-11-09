using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Entities.Orders
{
    // Table
    public class Order : BaseEntity<Guid>
    {
        public Order()
        {
            
        }
        public Order(string userEmail, OrderAddress shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal)
        {
            UserEmail = userEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
        }

        public string UserEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public OrderAddress ShippingAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; } // Navigational Property
        public int DeliveryMethodId { get; set; } // FK
        public ICollection<OrderItem> Items { get; set; } // Navigational Property
        public decimal SubTotal { get; set; } // Price * Quantity

        //[NotMapped]
        //public decimal Total { get; set; } // SubTotal + Delivery Method Cost

        public decimal GetTotal() => SubTotal + DeliveryMethod.Price; // Not Mapped 
    }
}
