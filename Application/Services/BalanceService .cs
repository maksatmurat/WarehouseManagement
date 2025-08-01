using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Repositories.Contracts;

namespace Application.Services;

public class BalanceService : GenericService<Balance>, IBalanceService
{
    public BalanceService(IGenericRepository<Balance> repo) : base(repo) { }
}
