using Domain.Entities;

namespace Application.Interfaces;

public interface IShipmentDocumentService : IGenericService<ShipmentDocument>
{
    Task<bool> SignDocumentAsync(Guid documentId);
    Task<bool> CancelDocumentAsync(Guid documentId);
}
