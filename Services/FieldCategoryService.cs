using AutoMapper;
using IdentityVerification.Api.DTOs;
using IdentityVerification.Api.Entities;
using IdentityVerification.Api.Repositories;
using IdentityVerification.Api.Services.Interfaces;

namespace IdentityVerification.Api.Services
{
    public class FieldCategoryService : IFieldCategoryService
    {
        private readonly IGenericRepository<FieldCategory> _repo;
        private readonly IMapper _mapper;

        public FieldCategoryService(IGenericRepository<FieldCategory> repo, IMapper mapper)
        {
            _repo = repo; _mapper = mapper;
        }

        public async Task<IReadOnlyList<FieldCategoryDto>> GetAllAsync(CancellationToken ct = default)
            => _mapper.Map<IReadOnlyList<FieldCategoryDto>>(await _repo.GetAllAsync(ct));

        public async Task<FieldCategoryDto?> GetAsync(int id, CancellationToken ct = default)
            => _mapper.Map<FieldCategoryDto?>(await _repo.GetByIdAsync(id, ct));

        public async Task<FieldCategoryDto> CreateAsync(CreateFieldCategoryDto dto, CancellationToken ct = default)
        {
            var entity = _mapper.Map<FieldCategory>(dto);
            await _repo.AddAsync(entity, ct);
            await _repo.SaveChangesAsync(ct);
            return _mapper.Map<FieldCategoryDto>(entity);
        }

        public async Task<FieldCategoryDto?> UpdateAsync(int id, UpdateFieldCategoryDto dto, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            if (entity == null) return null;
            _mapper.Map(dto, entity);
            _repo.Update(entity);
            await _repo.SaveChangesAsync(ct);
            return _mapper.Map<FieldCategoryDto>(entity);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            if (entity == null) return false;
            _repo.Remove(entity);
            await _repo.SaveChangesAsync(ct);
            return true;
        }
    }
}
