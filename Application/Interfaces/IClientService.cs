using Domain.Entities;

namespace Application.Interfaces;

public interface IClientService : IGenericService<Client>
{
    Task<List<Client>> GetActiveClientsAsync();
}
