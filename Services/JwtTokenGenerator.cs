using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace IdentityVerification.Api.Infrastructure
{
    /// <summary>
    /// Small helper to issue JWTs that match the validation configured in Program.cs
    /// </summary>
    public class JwtTokenGenerator
    {
        private readonly SymmetricSecurityKey _signingKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expiresMinutes;

        public JwtTokenGenerator(string key, string issuer, string audience, int expiresMinutes)
        {
            _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            _issuer = issuer;
            _audience = audience;
            _expiresMinutes = expiresMinutes;
        }

        /// <summary>
        /// Generates a signed JWT.
        /// </summary>
        /// <param name="email">User email (added as claim)</param>
        /// <param name="role">User role (added as Role claim)</param>
        /// <param name="userId">User ID (added as 'sub')</param>
        /// <returns>JWT string</returns>
        public string GenerateToken(string email, string role, string userId)
        {
            var nowUtc = DateTime.UtcNow;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Email, email ?? string.Empty),
                new Claim(ClaimTypes.Role, role ?? "User"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var creds = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                notBefore: nowUtc,
                expires: nowUtc.AddMinutes(_expiresMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
