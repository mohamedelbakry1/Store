using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Store.Shared.DTOS.Identity
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Email is Required"),EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = default!;
        [Required(ErrorMessage = "Display Name is Required")]
        public string DisplayName { get; set; } = default!;
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; } = default!;
        [Required(ErrorMessage = "User Name is Required")]
        public string UserName { get; set; } = default!;
        [Required(ErrorMessage = "Phone Number is Required")]
        public string PhoneNumber { get; set; } = default!;
    }
}
