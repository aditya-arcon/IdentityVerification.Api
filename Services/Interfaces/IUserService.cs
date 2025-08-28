// IUserService.cs
using IdentityVerification.Api.DTOs;

namespace IdentityVerification.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<IReadOnlyList<UserDto>> GetAllAsync(CancellationToken ct = default);
        Task<UserDto?> GetAsync(int id, CancellationToken ct = default);
        Task<UserDto> CreateAsync(CreateUserDto dto, CancellationToken ct = default);
        Task<UserDto?> UpdateAsync(int id, UpdateUserDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);
        Task<UserDto?> GetByEmailAsync(string email, CancellationToken ct = default);
    }
}
