using IdentityVerification.Api.DTOs;

namespace IdentityVerification.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponseDto> IssueTokenAsync(TokenRequestDto request, CancellationToken ct = default);
    }
}
