using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Repositories;

public class GenericRepository<TEntity,TContext>: IGenericRepository<TEntity>
    where TEntity : class
    where TContext : DbContext
{
    protected readonly TContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(TContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<TEntity>();
    }
    public IQueryable<TEntity> Query() => _dbSet.AsQueryable();

    public async Task<List<TEntity>> GetAllAsync() => await _dbSet.AsNoTracking().ToListAsync();

    public async Task<TEntity?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

    public async Task AddAsync(TEntity entity) => await _dbSet.AddAsync(entity);

    public void Update(TEntity entity) => _dbSet.Update(entity);
    public void Detach(TEntity entity)
    {
        var entry = _context.Entry(entity);
        if (entry != null)
            entry.State = EntityState.Detached;
    }

    public void SetValues(TEntity existingEntity, TEntity updatedEntity)
    {
        _context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
    }
    public void Remove(TEntity entity) => _dbSet.Remove(entity);

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate) =>
        await _dbSet.AnyAsync(predicate);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

   
}
