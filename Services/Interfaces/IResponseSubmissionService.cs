// IResponseSubmissionService.cs
using IdentityVerification.Api.DTOs;

namespace IdentityVerification.Api.Services.Interfaces
{
    public interface IResponseSubmissionService
    {
        Task<IReadOnlyList<ResponseSubmissionDto>> GetAllAsync(CancellationToken ct = default);
        Task<ResponseSubmissionDto?> GetAsync(int id, CancellationToken ct = default);
        Task<ResponseSubmissionDto> CreateAsync(CreateResponseSubmissionDto dto, CancellationToken ct = default);
        Task<ResponseSubmissionDto?> UpdateAsync(int id, UpdateResponseSubmissionDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}
