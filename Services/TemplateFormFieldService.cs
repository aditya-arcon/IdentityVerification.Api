using AutoMapper;
using IdentityVerification.Api.DTOs;
using IdentityVerification.Api.Entities;
using IdentityVerification.Api.Repositories;
using IdentityVerification.Api.Services.Interfaces;

namespace IdentityVerification.Api.Services
{
    public class TemplateFormFieldService : ITemplateFormFieldService
    {
        private readonly IGenericRepository<TemplateFormField> _repo;
        private readonly IMapper _mapper;

        public TemplateFormFieldService(IGenericRepository<TemplateFormField> repo, IMapper mapper)
        {
            _repo = repo; _mapper = mapper;
        }

        public async Task<IReadOnlyList<TemplateFormFieldDto>> GetAllAsync(CancellationToken ct = default)
            => _mapper.Map<IReadOnlyList<TemplateFormFieldDto>>(await _repo.GetAllAsync(ct));

        public async Task<TemplateFormFieldDto?> GetAsync(int id, CancellationToken ct = default)
            => _mapper.Map<TemplateFormFieldDto?>(await _repo.GetByIdAsync(id, ct));

        public async Task<TemplateFormFieldDto> CreateAsync(CreateTemplateFormFieldDto dto, CancellationToken ct = default)
        {
            var entity = _mapper.Map<TemplateFormField>(dto);
            await _repo.AddAsync(entity, ct);
            await _repo.SaveChangesAsync(ct);
            return _mapper.Map<TemplateFormFieldDto>(entity);
        }

        public async Task<TemplateFormFieldDto?> UpdateAsync(int id, UpdateTemplateFormFieldDto dto, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            if (entity == null) return null;
            _mapper.Map(dto, entity);
            _repo.Update(entity);
            await _repo.SaveChangesAsync(ct);
            return _mapper.Map<TemplateFormFieldDto>(entity);
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
