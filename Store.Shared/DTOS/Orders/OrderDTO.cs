
using Store.Shared.DTOS.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Store.Shared.DTOS.Orders
{
    public class OrderDTO
    {
        [Required(ErrorMessage= "BasketId is Required")]
        public string BasketId { get; set; } = default!;
        [Required(ErrorMessage = "DeliveryMethod is Required")]
        public int DeliveryMethodId { get; set; } = default!;
        [Required(ErrorMessage = "ShippingAddress is Required")]
        public AddressDTO ShippingAddress { get; set; } = default!;
    }
}
