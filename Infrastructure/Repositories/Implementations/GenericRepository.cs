using Application.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Implementations;

public class GenericRepository<TEntity>
        : GenericRepository<TEntity, WarehouseDbContext>, IGenericRepository<TEntity>
        where TEntity : class
{
    public GenericRepository(WarehouseDbContext context)
        : base(context)
    {
    }
}