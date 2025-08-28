using AutoMapper;
using IdentityVerification.Api.DTOs;
using IdentityVerification.Api.Entities;
using IdentityVerification.Api.Repositories;
using IdentityVerification.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityVerification.Api.Services
{
    public class ResponseSubmissionService : IResponseSubmissionService
    {
        private readonly IGenericRepository<ResponseSubmission> _repo;
        private readonly IGenericRepository<User> _users;
        private readonly IGenericRepository<Template> _templates;
        private readonly IMapper _mapper;

        public ResponseSubmissionService(
            IGenericRepository<ResponseSubmission> repo,
            IGenericRepository<User> users,
            IGenericRepository<Template> templates,
            IMapper mapper)
        {
            _repo = repo; _users = users; _templates = templates; _mapper = mapper;
        }

        public async Task<IReadOnlyList<ResponseSubmissionDto>> GetAllAsync(CancellationToken ct = default)
            => _mapper.Map<IReadOnlyList<ResponseSubmissionDto>>(await _repo.GetAllAsync(ct));

        public async Task<ResponseSubmissionDto?> GetAsync(int id, CancellationToken ct = default)
            => _mapper.Map<ResponseSubmissionDto?>(await _repo.GetByIdAsync(id, ct));

        public async Task<ResponseSubmissionDto> CreateAsync(CreateResponseSubmissionDto dto, CancellationToken ct = default)
        {
            var userExists = await _users.Query().AnyAsync(u => u.UserID == dto.UserID, ct);
            var templateExists = await _templates.Query().AnyAsync(t => t.TemplateID == dto.TemplateID, ct);
            if (!userExists || !templateExists) throw new InvalidOperationException("User or Template not found.");

            var entity = _mapper.Map<ResponseSubmission>(dto);
            entity.SubmittedAt = DateTime.UtcNow;

            await _repo.AddAsync(entity, ct);
            await _repo.SaveChangesAsync(ct);
            return _mapper.Map<ResponseSubmissionDto>(entity);
        }

        public async Task<ResponseSubmissionDto?> UpdateAsync(int id, UpdateResponseSubmissionDto dto, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            if (entity == null) return null;
            _mapper.Map(dto, entity);
            _repo.Update(entity);
            await _repo.SaveChangesAsync(ct);
            return _mapper.Map<ResponseSubmissionDto>(entity);
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
