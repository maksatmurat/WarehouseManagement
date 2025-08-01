using Infrastructure.Data;
using Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Implementations;

public class GenericRepository<TEntity>: IGenericRepository<TEntity>
    where TEntity : class
{
    protected readonly WarehouseDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(WarehouseDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }
    public IQueryable<TEntity> Query() => _dbSet.AsQueryable();

    public async Task<List<TEntity>> GetAllAsync() => await _dbSet.AsNoTracking().ToListAsync();

    public async Task<TEntity?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

    public async Task AddAsync(TEntity entity) => await _dbSet.AddAsync(entity);

    public void Update(TEntity entity) => _dbSet.Update(entity);

    public void Remove(TEntity entity) => _dbSet.Remove(entity);

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate) =>
        await _dbSet.AnyAsync(predicate);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

   
}
