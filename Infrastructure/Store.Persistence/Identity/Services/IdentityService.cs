using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Persistence.Identity.Entities;
using Store.Services.Abstractions.Auth;
using Store.Shared.Common;
using Store.Shared.DTOS.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Persistence.Identity.Services
{
    internal class IdentityService(UserManager<AppUser> _userManager) : IIdentityService
    {
        public async Task<Result<bool>> CheckPasswordAsync(string email, string password, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Error.NotFound("User.NotFound", $"User with email {email} is not found");
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<Result<IdentityUserResult>> CreateUserAsync(RegisterDTO register, CancellationToken ct = default)
        {
            var user = new AppUser()
            {
                Email = register.Email,
                DisplayName = register.DisplayName,
                UserName = register.UserName,
                PhoneNumber = register.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => new Error(e.Code, e.Description, ErrorType.Failure)).ToList();
                return Result<IdentityUserResult>.Fail(errors);
            }

            return new IdentityUserResult(user.Id, user.Email, user.DisplayName, user.UserName);
        }

        public async Task<Result<bool>> EmailExistsAsync(string email, CancellationToken ct = default)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        public async Task<Result<IdentityUserResult>> FindUserByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Error.NotFound("User.NotFound", $"User with email {email} is not found");

            return new IdentityUserResult(user.Id, user.Email!, user.DisplayName, user.UserName!);
        }

        public async Task<Result<AddressDTO>> GetUserAddressAsync(string email, CancellationToken ct = default)
        {
            var user = await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email, ct);
            if (user?.Address is null)
                return Error.NotFound("UserAddress.NotFound", $"Address of user with {email} is not found");
            var address = user.Address;
            return new AddressDTO
            {
                FirstName = address.FirstName,
                LastName = address.LastName,
                Street = address.Street,
                City = address.City,
                Country = address.Country
            };
        }

        public async Task<Result<IReadOnlyList<string>>> GetUserRolesAsync(string email, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Error.NotFound("User.NotFound", $"User with email {email} is not found");

            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<Result<AddressDTO>> UpdateOrInsertUserAddress(string email, AddressDTO addressDTO, CancellationToken ct = default)
        {
            var user = await _userManager.Users.Include(a => a.Address).FirstOrDefaultAsync(u => u.Email == email, ct);

            if (user?.Address is null)
            {
                user!.Address = new Address
                {
                    FirstName = addressDTO.FirstName,
                    LastName = addressDTO.LastName,
                    Street = addressDTO.Street,
                    City = addressDTO.City,
                    Country = addressDTO.Country
                };
            }
            else
            {
                user.Address.FirstName = addressDTO.FirstName;
                user.Address.LastName = addressDTO.LastName;
                user.Address.Street = addressDTO.Street;
                user.Address.City = addressDTO.City;
                user.Address.Country = addressDTO.Country;
            }

            var result = await _userManager.UpdateAsync(user!);
            if (result.Succeeded)
                return addressDTO;
            else
                return Error.Failure("UserAddressUpdate.Failure", string.Join(";", result.Errors.Select(e => e.Description)));
        }
    }
}
