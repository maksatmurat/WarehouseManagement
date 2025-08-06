using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;

namespace Application.Services;

public class BalanceService : GenericService<Balance>, IBalanceService
{
    public BalanceService(IGenericRepository<Balance> repository)
        : base(repository) { }
}