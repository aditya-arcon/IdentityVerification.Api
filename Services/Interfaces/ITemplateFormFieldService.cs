// ITemplateFormFieldService.cs
using IdentityVerification.Api.DTOs;

namespace IdentityVerification.Api.Services.Interfaces
{
    public interface ITemplateFormFieldService
    {
        Task<IReadOnlyList<TemplateFormFieldDto>> GetAllAsync(CancellationToken ct = default);
        Task<TemplateFormFieldDto?> GetAsync(int id, CancellationToken ct = default);
        Task<TemplateFormFieldDto> CreateAsync(CreateTemplateFormFieldDto dto, CancellationToken ct = default);
        Task<TemplateFormFieldDto?> UpdateAsync(int id, UpdateTemplateFormFieldDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}
