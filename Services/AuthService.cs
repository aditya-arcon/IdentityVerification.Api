using IdentityVerification.Api.DTOs;
using IdentityVerification.Api.Repositories;
using IdentityVerification.Api.Entities;
using IdentityVerification.Api.Services.Interfaces;
using IdentityVerification.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace IdentityVerification.Api.Services
{
    /// <summary>
    /// Demo-only auth service: verifies the user exists and issues a JWT with their role.
    /// Replace with your IdP or credential store as needed.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _users;
        private readonly JwtTokenGenerator _jwt;

        public AuthService(IGenericRepository<User> users, JwtTokenGenerator jwt)
        {
            _users = users;
            _jwt = jwt;
        }

        public async Task<TokenResponseDto> IssueTokenAsync(TokenRequestDto request, CancellationToken ct = default)
        {
            var user = await _users.Query().FirstOrDefaultAsync(u => u.Email == request.Email, ct);
            if (user == null) throw new InvalidOperationException("User not found.");

            var (token, expiresAt) = _jwt.Generate(user.Email, user.Role, user.UserName);
            return new TokenResponseDto { Token = token, ExpiresAtUtc = expiresAt };
        }
    }
}
