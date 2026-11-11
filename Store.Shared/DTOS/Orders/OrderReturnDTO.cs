
using Store.Shared.DTOS.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Shared.DTOS.Orders
{
    public class OrderReturnDTO
    {
        public Guid Id { get; set; }
        public string BuyerEmail { get; set; } = default!;
        public ICollection<OrderItemDTO> OrderItems { get; set; } = [];
        public AddressDTO ShippingAddress { get; set; } = default!;
        public string DeliveryMethod { get; set; } = default!;
        public decimal DeliveryMethodCost { get; set; }
        public string Status { get; set; } = default!;
        public DateTimeOffset OrderDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
    }
}
