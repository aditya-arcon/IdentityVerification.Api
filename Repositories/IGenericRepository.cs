using System.Linq.Expressions;

namespace IdentityVerification.Api.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> AddAsync(T entity, CancellationToken ct = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate,
                                     CancellationToken ct = default,
                                     params Expression<Func<T, object>>[] includes);
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default,
                                           params Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdAsync(object id, CancellationToken ct = default);
        void Update(T entity);
        void Remove(T entity);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        IQueryable<T> Query();
    }
}
