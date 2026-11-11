using Store.Shared.Common;
using Store.Shared.DTOS.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Abstractions.Auth
{
    public interface IIdentityService
    {
        Task<Result<IdentityUserResult>> FindUserByEmailAsync(string email, CancellationToken ct = default);
        Task<Result<bool>> CheckPasswordAsync(string email, string password, CancellationToken ct = default);
        Task<Result<IdentityUserResult>> CreateUserAsync(RegisterDTO register, CancellationToken ct = default);
        Task<Result<IReadOnlyList<string>>> GetUserRolesAsync(string email, CancellationToken ct = default);
        Task<Result<bool>> EmailExistsAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDTO>> GetUserAddressAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDTO>> UpdateOrInsertUserAddress(string email, AddressDTO addressDTO, CancellationToken ct = default);
    }
}
