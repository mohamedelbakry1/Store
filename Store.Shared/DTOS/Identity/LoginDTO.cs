using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Store.Shared.DTOS.Identity
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email is Required"), EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = default!;
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; } = default!;
    }
}
