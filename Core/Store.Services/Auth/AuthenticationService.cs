using Store.Services.Abstractions.Auth;
using Store.Shared.Common;
using Store.Shared.DTOS.Identity;

namespace Store.Services.Auth
{
    internal class AuthenticationService(IIdentityService _identityService, ITokenService _tokenService) : IAuthenticationService
    {
        public async Task<Result<bool>> CheckEmailExistsAsync(string email, CancellationToken ct = default)
        {
            return await _identityService.EmailExistsAsync(email);
        }

        public async Task<Result<AddressDTO>> GetCurrentUserAddressAsync(string email, CancellationToken ct = default)
        {
            return await _identityService.GetUserAddressAsync(email, ct);
        }

        public async Task<Result<UserDTO>> GetCurrnetUserAsync(string email, CancellationToken ct = default)
        {
            var userResult = await _identityService.FindUserByEmailAsync(email, ct);
            var user = userResult.Value;

            var rolesResult = await _identityService.GetUserRolesAsync(user.Email, ct);
            var roles = rolesResult.Value;

            var token = _tokenService.GenerateToken(user.Id, user.Email, user.UserName, roles);

            return new UserDTO
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = token
            };
        }

        public async Task<Result<UserDTO>> LoginAsync(LoginDTO login, CancellationToken ct = default)
        {
            var userResult = await _identityService.FindUserByEmailAsync(login.Email, ct);
            if (!userResult.IsSuccess)
                return Result<UserDTO>.Fail(userResult.Errors);

            var passwordResult = await _identityService.CheckPasswordAsync(login.Email, login.Password, ct);

            if (!passwordResult.IsSuccess)
                return Result<UserDTO>.Fail(userResult.Errors);
            if (!passwordResult.Value)
                return Error.Unauthorized("User.Login", "Invalid email or password");

            var user = userResult.Value;
            var rolesResult = await _identityService.GetUserRolesAsync(user.Email);
            var roles = rolesResult.Value;
            var token = _tokenService.GenerateToken(user.Id, user.Email, user.UserName, roles);

            return new UserDTO
            {
                DisplayName = userResult.Value.DisplayName,
                Email = userResult.Value.Email,
                Token = token
            };
        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO register, CancellationToken ct = default)
        {
            var userResult = await _identityService.CreateUserAsync(register, ct);

            if (!userResult.IsSuccess)
                return Result<UserDTO>.Fail(userResult.Errors);

            var user = userResult.Value;
            var rolesResult = await _identityService.GetUserRolesAsync(user.Email);
            var roles = rolesResult.Value;
            var token = _tokenService.GenerateToken(user.Id, user.Email, user.UserName, roles);

            return new UserDTO()
            {
                DisplayName = userResult.Value.DisplayName,
                Email = userResult.Value.Email,
                Token = token
            };
        }

        public async Task<Result<AddressDTO>> UpSertUserAddressAsync(string email, AddressDTO addressDTO, CancellationToken ct = default)
        {
            return await _identityService.UpdateOrInsertUserAddress(email, addressDTO, ct);
        }
    }
}
