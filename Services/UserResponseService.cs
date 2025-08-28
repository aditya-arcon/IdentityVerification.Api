using AutoMapper;
using IdentityVerification.Api.DTOs;
using IdentityVerification.Api.Entities;
using IdentityVerification.Api.Repositories;
using IdentityVerification.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityVerification.Api.Services
{
    public class UserResponseService : IUserResponseService
    {
        private readonly IGenericRepository<UserResponse> _repo;
        private readonly IGenericRepository<ResponseSubmission> _subs;
        private readonly IGenericRepository<FormField> _fields;
        private readonly IMapper _mapper;

        public UserResponseService(
            IGenericRepository<UserResponse> repo,
            IGenericRepository<ResponseSubmission> subs,
            IGenericRepository<FormField> fields,
            IMapper mapper)
        {
            _repo = repo; _subs = subs; _fields = fields; _mapper = mapper;
        }

        public async Task<IReadOnlyList<UserResponseDto>> GetAllAsync(CancellationToken ct = default)
            => _mapper.Map<IReadOnlyList<UserResponseDto>>(await _repo.GetAllAsync(ct));

        public async Task<UserResponseDto?> GetAsync(int id, CancellationToken ct = default)
            => _mapper.Map<UserResponseDto?>(await _repo.GetByIdAsync(id, ct));

        public async Task<UserResponseDto> CreateAsync(CreateUserResponseDto dto, CancellationToken ct = default)
        {
            // Ensure submission & field exist
            var exists = await _subs.Query().AnyAsync(s => s.SubmissionID == dto.SubmissionID, ct)
                         && await _fields.Query().AnyAsync(f => f.FieldID == dto.FieldID, ct);
            if (!exists) throw new InvalidOperationException("Submission or Field not found.");

            // Exactly one value check (defense in depth beyond attribute)
            int count = (dto.ValueText is null ? 0 : 1)
                        + (dto.ValueNumber is null ? 0 : 1)
                        + (dto.ValueDate is null ? 0 : 1)
                        + (dto.ValueFile is null ? 0 : 1);
            if (count != 1) throw new InvalidOperationException("Exactly one value must be provided.");

            var entity = _mapper.Map<UserResponse>(dto);
            await _repo.AddAsync(entity, ct);
            await _repo.SaveChangesAsync(ct);
            return _mapper.Map<UserResponseDto>(entity);
        }

        public async Task<UserResponseDto?> UpdateAsync(int id, UpdateUserResponseDto dto, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            if (entity == null) return null;

            int count = (dto.ValueText is null ? 0 : 1)
                        + (dto.ValueNumber is null ? 0 : 1)
                        + (dto.ValueDate is null ? 0 : 1)
                        + (dto.ValueFile is null ? 0 : 1);
            if (count != 1) throw new InvalidOperationException("Exactly one value must be provided.");

            _mapper.Map(dto, entity);
            _repo.Update(entity);
            await _repo.SaveChangesAsync(ct);
            return _mapper.Map<UserResponseDto>(entity);
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
