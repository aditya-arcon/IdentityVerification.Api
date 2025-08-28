using AutoMapper;
using IdentityVerification.Api.DTOs;
using IdentityVerification.Api.Entities;
using IdentityVerification.Api.Repositories;
using IdentityVerification.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityVerification.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _repo;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository<User> repo, IMapper mapper)
        {
            _repo = repo; _mapper = mapper;
        }

        public async Task<IReadOnlyList<UserDto>> GetAllAsync(CancellationToken ct = default)
            => _mapper.Map<IReadOnlyList<UserDto>>(await _repo.GetAllAsync(ct));

        public async Task<UserDto?> GetAsync(int id, CancellationToken ct = default)
            => _mapper.Map<UserDto?>(await _repo.GetByIdAsync(id, ct));

        public async Task<UserDto> CreateAsync(CreateUserDto dto, CancellationToken ct = default)
        {
            var exists = await _repo.Query().AnyAsync(u => u.Email == dto.Email, ct);
            if (exists) throw new InvalidOperationException("Email must be unique.");

            var entity = _mapper.Map<User>(dto);
            entity.CreatedAt = DateTime.UtcNow;

            await _repo.AddAsync(entity, ct);
            await _repo.SaveChangesAsync(ct);
            return _mapper.Map<UserDto>(entity);
        }

        public async Task<UserDto?> UpdateAsync(int id, UpdateUserDto dto, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            if (entity == null) return null;
            _mapper.Map(dto, entity);
            _repo.Update(entity);
            await _repo.SaveChangesAsync(ct);
            return _mapper.Map<UserDto>(entity);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            if (entity == null) return false;
            _repo.Remove(entity);
            await _repo.SaveChangesAsync(ct);
            return true;
        }

        public Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default)
            => _repo.Query().AnyAsync(u => u.Email == email, ct);

        public async Task<UserDto?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await _repo.Query().FirstOrDefaultAsync(u => u.Email == email, ct);
            return _mapper.Map<UserDto?>(user);
        }
    }
}
