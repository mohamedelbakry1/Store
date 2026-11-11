using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Persistence.Identity.Entities
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; } = default!;
        public Address? Address { get; set; }
    }
}
