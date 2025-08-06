using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;

namespace Application.Services;

public class ShipmentResourceService : GenericService<ShipmentResource>, IShipmentResourceService
{
    public ShipmentResourceService(IGenericRepository<ShipmentResource> repository)
        : base(repository) { }

    public async Task<bool> UpdateResourceQuantityAsync(Guid shipmentResourceId, decimal newQuantity)
    {
        var resource = await _repository.GetByIdAsync(shipmentResourceId);
        if (resource == null) return false;

        resource.Quantity = newQuantity;
        _repository.Update(resource);
        await _repository.SaveChangesAsync();
        return true;
    }
}