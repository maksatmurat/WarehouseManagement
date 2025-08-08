namespace Application.Interfaces;

public interface IGenericService<TEntity> where TEntity : class
{
    Task<List<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<TEntity> CreateAsync(TEntity entity);
    Task<TEntity> UpdateAsync(Guid id, TEntity entity);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsByNameAsync(string name);
}
