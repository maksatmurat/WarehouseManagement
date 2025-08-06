using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;

namespace Application.Services;

public class ClientService : GenericService<Client>, IClientService
{
    private readonly IGenericRepository<Client> _clientRepo;
    private readonly IGenericRepository<ShipmentDocument> _shipmentRepo;

    public ClientService(
        IGenericRepository<Client> clientRepo,
        IGenericRepository<ShipmentDocument> shipmentRepo
    ) : base(clientRepo)
    {
        _clientRepo = clientRepo;
        _shipmentRepo = shipmentRepo;
    }

    public override async Task<Client> CreateAsync(Client entity)
    {
        bool exists = await _clientRepo.ExistsAsync(c => c.Name == entity.Name && c.IsActive);
        if (exists)
            throw new InvalidOperationException("Клиент с таким именем уже существует.");

        return await base.CreateAsync(entity);
    }

    public override async Task<Client> UpdateAsync(Guid id, Client entity)
    {
        bool exists = await _clientRepo.ExistsAsync(c => c.Id != id && c.Name == entity.Name && c.IsActive);
        if (exists)
            throw new InvalidOperationException("Клиент с таким именем уже существует.");

        return await base.UpdateAsync(id, entity);
    }

    public override async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _clientRepo.GetByIdAsync(id);
        if (entity == null) return false;

        bool isUsed = await _shipmentRepo.ExistsAsync(s => s.ClientId == id);

        if (isUsed)
        {
            entity.IsActive = false;
            _clientRepo.Update(entity);
        }
        else
        {
            _clientRepo.Remove(entity);
        }

        await _clientRepo.SaveChangesAsync();
        return true;
    }

    
}

