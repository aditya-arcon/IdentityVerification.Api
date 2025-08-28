using IdentityVerification.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IdentityVerification.Api.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        private readonly DbSet<T> _set;

        public GenericRepository(AppDbContext db)
        {
            _db = db;
            _set = db.Set<T>();
        }

        public async Task<T> AddAsync(T entity, CancellationToken ct = default)
        {
            await _set.AddAsync(entity, ct);
            return entity;
        }

        public Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default)
            => _set.AddRangeAsync(entities, ct);

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _set.AsQueryable();
            foreach (var inc in includes) query = query.Include(inc);
            return await query.FirstOrDefaultAsync(predicate, ct);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _set.AsQueryable();
            foreach (var inc in includes) query = query.Include(inc);
            return await query.ToListAsync(ct);
        }

        public Task<T?> GetByIdAsync(object id, CancellationToken ct = default) => _set.FindAsync(new object?[] { id }, ct).AsTask();

        public void Update(T entity) => _set.Update(entity);

        public void Remove(T entity) => _set.Remove(entity);

        public Task<int> SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);

        public IQueryable<T> Query() => _set.AsQueryable();
    }
}
