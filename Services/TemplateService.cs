using AutoMapper;
using IdentityVerification.Api.DTOs;
using IdentityVerification.Api.Entities;
using IdentityVerification.Api.Repositories;
using IdentityVerification.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityVerification.Api.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly IGenericRepository<Template> _repo;
        private readonly IGenericRepository<User> _users;
        private readonly IMapper _mapper;

        public TemplateService(IGenericRepository<Template> repo, IGenericRepository<User> users, IMapper mapper)
        {
            _repo = repo;
            _users = users;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<TemplateDto>> GetAllAsync(CancellationToken ct = default)
        {
            var list = await _repo.GetAllAsync(ct);
            return _mapper.Map<IReadOnlyList<TemplateDto>>(list);
        }

        public async Task<TemplateDto?> GetAsync(int id, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            return entity == null ? null : _mapper.Map<TemplateDto>(entity);
        }

        public async Task<TemplateDto> CreateAsync(CreateTemplateDto dto, CancellationToken ct = default)
        {
            var creatorExists = await _users.Query().AnyAsync(u => u.Email == dto.CreatedBy, ct);
            if (!creatorExists) throw new InvalidOperationException("CreatedBy email must belong to an existing user.");

            var entity = _mapper.Map<Template>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.LastUpdated = entity.CreatedAt;

            await _repo.AddAsync(entity, ct);
            await _repo.SaveChangesAsync(ct);
            return _mapper.Map<TemplateDto>(entity);
        }

        public async Task<TemplateDto?> UpdateAsync(int id, UpdateTemplateDto dto, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            if (entity == null) return null;

            _mapper.Map(dto, entity);
            entity.LastUpdated = DateTime.UtcNow;

            _repo.Update(entity);
            await _repo.SaveChangesAsync(ct);
            return _mapper.Map<TemplateDto>(entity);
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
