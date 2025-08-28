// ITemplateService.cs
using IdentityVerification.Api.DTOs;

namespace IdentityVerification.Api.Services.Interfaces
{
    public interface ITemplateService
    {
        Task<IReadOnlyList<TemplateDto>> GetAllAsync(CancellationToken ct = default);
        Task<TemplateDto?> GetAsync(int id, CancellationToken ct = default);
        Task<TemplateDto> CreateAsync(CreateTemplateDto dto, CancellationToken ct = default);
        Task<TemplateDto?> UpdateAsync(int id, UpdateTemplateDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}
