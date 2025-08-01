using Domain.Entities;

namespace Application.Interfaces;

public interface IShipmentService : IGenericService<ShipmentDocument>
{
    Task<bool> SignShipmentAsync(Guid shipmentId);
    Task<bool> RevokeShipmentAsync(Guid shipmentId);
}
