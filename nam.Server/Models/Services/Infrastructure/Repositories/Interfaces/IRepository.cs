using System.Linq.Expressions;

namespace nam.Server.Models.Services.Infrastructure.Repositories.Interfaces
{
    public interface IRepository<TEntity, TKey> where TEntity : class
    {
        Task<TEntity?> GetAsync(TKey id, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        void Remove(TEntity entity);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
