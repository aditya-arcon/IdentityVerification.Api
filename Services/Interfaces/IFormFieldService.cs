// IFormFieldService.cs
using IdentityVerification.Api.DTOs;

namespace IdentityVerification.Api.Services.Interfaces
{
    public interface IFormFieldService
    {
        Task<IReadOnlyList<FormFieldDto>> GetAllAsync(CancellationToken ct = default);
        Task<FormFieldDto?> GetAsync(int id, CancellationToken ct = default);
        Task<FormFieldDto> CreateAsync(CreateFormFieldDto dto, CancellationToken ct = default);
        Task<FormFieldDto?> UpdateAsync(int id, UpdateFormFieldDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}
