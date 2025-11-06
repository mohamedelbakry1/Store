using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Store.Domain.Entities.Identity;
using Store.Domain.Exceptions.BadRequest;
using Store.Domain.Exceptions.NotFound;
using Store.Domain.Exceptions.UnAuthorized;
using Store.Services.Abstractions.Auth;
using Store.Shared;
using Store.Shared.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Auth
{
    public class AuthService(UserManager<AppUser> _userManager, IOptions<JwtOptions> options) : IAuthService
    {
        public async Task<UserResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null) throw new UserNotFoundException(request.Email);

            var flag = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!flag) throw new UnAuthorizedException();

            return new UserResponse()
            {
                DisplayName = user.DispalyName,
                Email = user.Email,
                Token = await GenerateTokenAsync(user)
            };
        }

        public async Task<UserResponse?> RegisterAsync(RegisterRequest request)
        {
            var user = new AppUser()
            {
                DispalyName = request.DisplayName,
                Email = request.Email,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded) throw new RegisterationBadRequestException(result.Errors.Select(E => E.Description).ToList());

            return new UserResponse()
            {
                DisplayName = user.DispalyName,
                Email = user.Email,
                Token = await GenerateTokenAsync(user)
            };
        }

        private async Task<string> GenerateTokenAsync(AppUser user)
        {
            // Token
            // 1. Header     (type, Algorithm)
            // 2. Payload    (Claims)
            // 3. Signature  (Key)

            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName, user.DispalyName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach(var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtOptions = options.Value;

            //STRONGSecurityKEYFORAUTHenticationSTRONGSecurityKEYFORAUTHenticationSTRONGSecurityKEYFORAUTHenticationSTRONGSecurityKEYFORAUTHentication
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKey));

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: authClaims,
                expires: DateTime.Now.AddDays(jwtOptions.DurationInDays),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
