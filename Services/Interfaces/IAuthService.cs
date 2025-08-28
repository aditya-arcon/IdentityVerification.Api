using System.Threading;
using System.Threading.Tasks;
using IdentityVerification.Api.DTOs;
using IdentityVerification.Api.Entities;

namespace IdentityVerification.Api.Services.Interfaces
{
    public interface IAuthService
    {
        // Keep legacy token-by-email path (expects a TokenRequestDto)
        Task<string> IssueTokenAsync(TokenRequestDto dto, CancellationToken ct = default);

        // New methods for email+password auth
        Task<User> RegisterAsync(RegisterUserWithPasswordDto dto, CancellationToken ct = default);
        Task<string> LoginWithPasswordAsync(LoginWithPasswordDto dto, CancellationToken ct = default);
        Task ChangePasswordAsync(int userId, ChangePasswordDto dto, CancellationToken ct = default);
    }
}
