using AutoMapper;
using IdentityVerification.Api.DTOs;
using IdentityVerification.Api.Entities;
using IdentityVerification.Api.Repositories;
using IdentityVerification.Api.Services.Interfaces;

namespace IdentityVerification.Api.Services
{
    public class FormFieldService : IFormFieldService
    {
        private readonly IGenericRepository<FormField> _repo;
        private readonly IMapper _mapper;

        public FormFieldService(IGenericRepository<FormField> repo, IMapper mapper)
        {
            _repo = repo; _mapper = mapper;
        }

        public async Task<IReadOnlyList<FormFieldDto>> GetAllAsync(CancellationToken ct = default)
            => _mapper.Map<IReadOnlyList<FormFieldDto>>(await _repo.GetAllAsync(ct));

        public async Task<FormFieldDto?> GetAsync(int id, CancellationToken ct = default)
            => _mapper.Map<FormFieldDto?>(await _repo.GetByIdAsync(id, ct));

        public async Task<FormFieldDto> CreateAsync(CreateFormFieldDto dto, CancellationToken ct = default)
        {
            var entity = _mapper.Map<FormField>(dto);
            await _repo.AddAsync(entity, ct);
            await _repo.SaveChangesAsync(ct);
            return _mapper.Map<FormFieldDto>(entity);
        }

        public async Task<FormFieldDto?> UpdateAsync(int id, UpdateFormFieldDto dto, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            if (entity == null) return null;
            _mapper.Map(dto, entity);
            _repo.Update(entity);
            await _repo.SaveChangesAsync(ct);
            return _mapper.Map<FormFieldDto>(entity);
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
