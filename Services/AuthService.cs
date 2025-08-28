using System;
using System.Threading;
using System.Threading.Tasks;
using IdentityVerification.Api.Data;
using IdentityVerification.Api.DTOs;
using IdentityVerification.Api.Entities;
using IdentityVerification.Api.Infrastructure;
using IdentityVerification.Api.Security;
using IdentityVerification.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityVerification.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly JwtTokenGenerator _jwt;
        private readonly IPasswordHasher _hasher;

        public AuthService(AppDbContext db, JwtTokenGenerator jwt, IPasswordHasher hasher)
        {
            _db = db;
            _jwt = jwt;
            _hasher = hasher;
        }

        // Legacy/compat: token-by-email
        public async Task<string> IssueTokenAsync(TokenRequestDto dto, CancellationToken ct = default)
        {
            var email = dto.Email?.Trim();
            if (string.IsNullOrEmpty(email))
                throw new InvalidOperationException("Email is required.");

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsActive, ct);
            if (user == null)
                throw new InvalidOperationException("User not found or inactive.");

            return _jwt.GenerateToken(user.Email, user.Role ?? "User", user.UserID.ToString());
        }

        // Register with email+password
        public async Task<User> RegisterAsync(RegisterUserWithPasswordDto dto, CancellationToken ct = default)
        {
            var email = dto.Email?.Trim() ?? string.Empty;

            var exists = await _db.Users.AnyAsync(u => u.Email == email, ct);
            if (exists)
                throw new InvalidOperationException("Email already registered.");

            _hasher.CreateHash(dto.Password, out var hash, out var salt);

            var role = string.IsNullOrWhiteSpace(dto.Role) ? "User" : dto.Role!.Trim();

            var user = new User
            {
                UserName = dto.UserName.Trim(),
                Email = email,
                Phone = string.IsNullOrWhiteSpace(dto.Phone) ? null : dto.Phone!.Trim(),
                Role = role,
                CreatedAt = DateTime.UtcNow,
                PasswordHash = hash,
                PasswordSalt = salt,
                PasswordUpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync(ct);
            return user;
        }

        // Login with email+password
        public async Task<string> LoginWithPasswordAsync(LoginWithPasswordDto dto, CancellationToken ct = default)
        {
            var email = dto.Email?.Trim() ?? string.Empty;

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
            if (user == null || !user.IsActive)
                throw new InvalidOperationException("Invalid credentials.");

            if (string.IsNullOrEmpty(user.PasswordHash) || string.IsNullOrEmpty(user.PasswordSalt))
                throw new InvalidOperationException("Password not set. Contact admin.");

            var ok = _hasher.Verify(dto.Password, user.PasswordHash!, user.PasswordSalt!);
            if (!ok)
                throw new InvalidOperationException("Invalid credentials.");

            return _jwt.GenerateToken(user.Email, user.Role ?? "User", user.UserID.ToString());
        }

        // Change password (authorized user)
        public async Task ChangePasswordAsync(int userId, ChangePasswordDto dto, CancellationToken ct = default)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserID == userId && u.IsActive, ct);
            if (user == null)
                throw new InvalidOperationException("User not found or inactive.");

            if (string.IsNullOrEmpty(user.PasswordHash) || string.IsNullOrEmpty(user.PasswordSalt))
                throw new InvalidOperationException("Password not set. Contact admin.");

            var ok = _hasher.Verify(dto.CurrentPassword, user.PasswordHash!, user.PasswordSalt!);
            if (!ok)
                throw new InvalidOperationException("Current password is incorrect.");

            _hasher.CreateHash(dto.NewPassword, out var newHash, out var newSalt);
            user.PasswordHash = newHash;
            user.PasswordSalt = newSalt;
            user.PasswordUpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(ct);
        }
    }
}
