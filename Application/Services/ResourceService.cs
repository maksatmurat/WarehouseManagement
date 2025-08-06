
using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;

namespace Application.Services;

public class ResourceService : GenericService<Resource>, IResourceService
{
    private readonly IGenericRepository<Resource> _repo;
    private readonly IGenericRepository<Balance> _balanceRepo;
    private readonly IGenericRepository<ReceiptResource> _receiptResRepo;
    private readonly IGenericRepository<ShipmentResource> _shipmentResRepo;

    public ResourceService(
    IGenericRepository<Resource> repo,
    IGenericRepository<Balance> balanceRepo,
    IGenericRepository<ReceiptResource> receiptResRepo,
    IGenericRepository<ShipmentResource> shipmentResRepo): base(repo)
    {
        _repo = repo;
        _balanceRepo = balanceRepo;
        _receiptResRepo = receiptResRepo;
        _shipmentResRepo = shipmentResRepo;
    }

    public override async Task<List<Resource>> GetAllAsync() => await _repo.GetAllAsync();

    public override async Task<Resource?> GetByIdAsync(Guid id) => await _repo.GetByIdAsync(id);

    public override async Task<Resource> CreateAsync(Resource entity)
    {
        bool exists = await _repo.ExistsAsync(r => r.Name == entity.Name && r.IsActive);
        if (exists)
            throw new InvalidOperationException("Ресурс с таким именем уже существует.");

        await _repo.AddAsync(entity);
        await _repo.SaveChangesAsync();
        return entity;
    }

    public override async Task<Resource> UpdateAsync(Guid id, Resource entity)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) throw new KeyNotFoundException("Ресурс не найден.");

        bool exists = await _repo.ExistsAsync(r => r.Id != id && r.Name == entity.Name && r.IsActive);
        if (exists)
            throw new InvalidOperationException("Ресурс с таким именем уже существует.");

        existing.Name = entity.Name;
        existing.IsActive = entity.IsActive;

        _repo.Update(existing);
        await _repo.SaveChangesAsync();
        return existing;
    }

    public override async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;

        // Проверяем, используется ли ресурс
        bool isUsed =
            await _balanceRepo.ExistsAsync(b => b.ResourceId == id) ||
            await _receiptResRepo.ExistsAsync(rr => rr.ResourceId == id) ||
            await _shipmentResRepo.ExistsAsync(sr => sr.ResourceId == id);

        if (isUsed)
        {
            // Переводим в архив
            entity.IsActive = false;
            _repo.Update(entity);
        }
        else
        {
            _repo.Remove(entity);
        }

        await _repo.SaveChangesAsync();
        return true;
    }

}
