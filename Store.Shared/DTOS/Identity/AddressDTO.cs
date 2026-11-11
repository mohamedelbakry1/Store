using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Store.Shared.DTOS.Identity
{
    public class AddressDTO
    {
        [Required(ErrorMessage = "Street is Required")]
        public string Street { get; set; } = default!;
        [Required(ErrorMessage = "City is Required")]
        public string City { get; set; } = default!;
        [Required(ErrorMessage = "Country is Required")]
        public string Country { get; set; } = default!;
        [Required(ErrorMessage = "First Name is Required")]
        public string FirstName { get; set; } = default!;
        [Required(ErrorMessage = "Last Name is Required")]
        public string LastName { get; set; } = default!;
    }
}
