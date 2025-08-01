using Domain.Entities;

namespace Application.Interfaces;

public interface IShipmentResourceService : IGenericService<ShipmentResource>
{
    Task<bool> UpdateResourceQuantityAsync(Guid shipmentResourceId, decimal newQuantity);
}
