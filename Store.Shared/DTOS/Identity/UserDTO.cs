using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Shared.DTOS.Identity
{
    public class UserDTO
    {
        public string DisplayName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Token { get; set; } = default!;
    }
}
