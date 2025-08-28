// IFieldCategoryService.cs
using IdentityVerification.Api.DTOs;

namespace IdentityVerification.Api.Services.Interfaces
{
    public interface IFieldCategoryService
    {
        Task<IReadOnlyList<FieldCategoryDto>> GetAllAsync(CancellationToken ct = default);
        Task<FieldCategoryDto?> GetAsync(int id, CancellationToken ct = default);
        Task<FieldCategoryDto> CreateAsync(CreateFieldCategoryDto dto, CancellationToken ct = default);
        Task<FieldCategoryDto?> UpdateAsync(int id, UpdateFieldCategoryDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}
