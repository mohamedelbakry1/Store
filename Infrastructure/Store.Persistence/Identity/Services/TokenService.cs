using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Store.Services.Abstractions.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Store.Persistence.Identity.Services
{
    internal class TokenService(IOptions<JwtSettings> _jwtSettings) : ITokenService
    {
        public string GenerateToken(string userId, string email, string userName, IReadOnlyList<string> roles)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Email, email),
            };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var secKey = _jwtSettings.Value.SecurityKey;
            if (string.IsNullOrWhiteSpace(secKey))
                throw new InvalidOperationException("Secret Key is not found");

            if (secKey.Length < 32)
                throw new InvalidOperationException("Secret Key is too short");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secKey));

            var siginngCredintails = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                issuer: _jwtSettings.Value.Issuer,
                audience: _jwtSettings.Value.Audience,
                claims: claims,
                signingCredentials: siginngCredintails,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.Value.ExpirationMinutes)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class JwtSettings
    {
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public string SecurityKey { get; set; } = default!;
        public int ExpirationMinutes { get; set; } = default!;
    }
}
