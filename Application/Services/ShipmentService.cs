using Application.DTOs;
using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ShipmentService : IShipmentService
{
    private readonly IGenericRepository<ShipmentResource> _shipmentResRepo;
    private readonly IGenericRepository<ShipmentDocument> _shipmentDocRepo;

    public ShipmentService(IGenericRepository<ShipmentResource> shipmentResRepo,IGenericRepository<ShipmentDocument> shipmentDocRepo )
    {
        _shipmentResRepo = shipmentResRepo;
        _shipmentDocRepo = shipmentDocRepo;
    }

    public async Task<List<ShipmentDocumentDto>> FilterAsync(ShipmentFilterDto filter)
    {
        var query = _shipmentResRepo.Query()
            .Include(sr => sr.ShipmentDocument)
                .ThenInclude(sd => sd.Client)
            .Include(sr => sr.Resource)
            .Include(sr => sr.UnitOfMeasure)
            .AsQueryable();

        if (filter.FromDate.HasValue)
            query = query.Where(sr => sr.ShipmentDocument.Date >= filter.FromDate.Value);

        if (filter.ToDate.HasValue)
            query = query.Where(sr => sr.ShipmentDocument.Date <= filter.ToDate.Value);

        if (!string.IsNullOrWhiteSpace(filter.Number))
            query = query.Where(sr => sr.ShipmentDocument.Number.Contains(filter.Number));

        if (filter.ClientId.HasValue)
            query = query.Where(sr => sr.ShipmentDocument.ClientId == filter.ClientId.Value);

        if (filter.ResourceId.HasValue)
            query = query.Where(sr => sr.ResourceId == filter.ResourceId.Value);

        if (filter.UnitOfMeasureId.HasValue)
            query = query.Where(sr => sr.UnitOfMeasureId == filter.UnitOfMeasureId.Value);

        return await query
            .OrderByDescending(sr => sr.ShipmentDocument.Date)
            .Select(sr => new ShipmentDocumentDto
            {
                Id = sr.ShipmentDocument.Id,
                Number = sr.ShipmentDocument.Number,
                Date = sr.ShipmentDocument.Date,
                ClientName = sr.ShipmentDocument.Client.Name,
                IsSigned = sr.ShipmentDocument.IsSigned,
                ResourceName = sr.Resource.Name,
                UnitOfMeasureName = sr.UnitOfMeasure.Name,
                Quantity = sr.Quantity
            })
            .ToListAsync();
    }
}