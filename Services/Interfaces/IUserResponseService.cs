// IUserResponseService.cs
using IdentityVerification.Api.DTOs;

namespace IdentityVerification.Api.Services.Interfaces
{
    public interface IUserResponseService
    {
        Task<IReadOnlyList<UserResponseDto>> GetAllAsync(CancellationToken ct = default);
        Task<UserResponseDto?> GetAsync(int id, CancellationToken ct = default);
        Task<UserResponseDto> CreateAsync(CreateUserResponseDto dto, CancellationToken ct = default);
        Task<UserResponseDto?> UpdateAsync(int id, UpdateUserResponseDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}
