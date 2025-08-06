using System.Linq.Expressions;

namespace Application.Repositories;

public interface IGenericRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> Query();
    Task<List<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(Guid id);
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    Task SaveChangesAsync();
    void Detach(TEntity entity);
    void SetValues(TEntity existingEntity, TEntity updatedEntity);

}