using System.Linq.Expressions;

namespace Play.Common
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {   
        Task<IReadOnlyCollection<TEntity>> GetAllAsync();
        Task<IReadOnlyCollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> GetByIdAsync(Guid id);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter);
        Task CreateAsync(TEntity item);
        Task UpdateAsync(TEntity item);
        Task RemoveAsync(Guid id);
    }
}
