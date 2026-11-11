using Store.Shared.Common;
using Store.Shared.DTOS.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Abstractions.Auth
{
    public interface IAuthenticationService
    {
        Task<Result<UserDTO>> LoginAsync(LoginDTO login, CancellationToken ct = default);
        Task<Result<UserDTO>> RegisterAsync(RegisterDTO register, CancellationToken ct = default);
        Task<Result<bool>> CheckEmailExistsAsync(string email, CancellationToken ct = default);
        Task<Result<UserDTO>> GetCurrnetUserAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDTO>> GetCurrentUserAddressAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDTO>> UpSertUserAddressAsync(string email, AddressDTO addressDTO, CancellationToken ct = default);
    }
}
