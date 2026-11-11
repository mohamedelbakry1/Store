using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Entities.Orders
{
    public class Order : BaseEntity<Guid>
    {
        private Order()
        {
            
        }
        public Order(string userEmail, OrderAddress shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal, string? paymentIntentId)
        {
            BuyerEmail = userEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = items;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        public string BuyerEmail { get; set; } = default!;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public OrderAddress ShippingAddress { get; set; } = default!;
        public DeliveryMethod DeliveryMethod { get; set; } = default!;
        public int DeliveryMethodId { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = [];
        public decimal SubTotal { get; set; }

        public decimal GetTotal() => SubTotal + DeliveryMethod.Cost;

        public string? PaymentIntentId { get; set; }
    }
}
