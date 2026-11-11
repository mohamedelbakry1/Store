using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Abstractions.Auth
{
    public interface ITokenService
    {
        string GenerateToken(string userId, string email, string userName, IReadOnlyList<string> roles);
    }
}
