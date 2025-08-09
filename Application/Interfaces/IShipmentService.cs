using Application.DTOs;

namespace Application.Interfaces;

public interface IShipmentService
{
    Task<List<ShipmentDocumentDto>> FilterAsync(ShipmentFilterDto filter);
}
