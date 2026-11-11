using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Shared.DTOS.Baskets
{
    public class CustomerBasketDTO
    {
        public string Id { get; set; } = default!;
        public ICollection<BasketItemDTO> BasketItems { get; set; } = [];
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal? ShippingPrice { get; set; }
    }
}
