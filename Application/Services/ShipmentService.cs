using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ShipmentService : GenericService<ShipmentDocument>, IShipmentService
{
    private readonly IGenericRepository<ShipmentDocument> _shipmentRepo;
    private readonly IGenericRepository<Balance> _balanceRepo;

    public ShipmentService(IGenericRepository<ShipmentDocument> shipmentRepo,
                           IGenericRepository<Balance> balanceRepo)
        : base(shipmentRepo)
    {
        _shipmentRepo = shipmentRepo;
        _balanceRepo = balanceRepo;
    }

    public async Task<bool> SignShipmentAsync(Guid shipmentId)
    {
        var shipment = await _shipmentRepo.GetByIdAsync(shipmentId);

        if (shipment == null) return false;

        foreach (var resource in shipment.ShipmentResources)
        {
            var balance = await _balanceRepo.Query()
                .FirstOrDefaultAsync(b => b.ResourceId == resource.ResourceId && b.UnitOfMeasureId == resource.UnitOfMeasureId);

            if (balance == null || balance.Quantity < resource.Quantity)
                throw new InvalidOperationException("Недостаточно ресурсов на складе.");

            balance.Quantity -= resource.Quantity;
            _balanceRepo.Update(balance);
        }

        await _balanceRepo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RevokeShipmentAsync(Guid shipmentId)
    {
        var shipment = await _shipmentRepo.GetByIdAsync(shipmentId);
        if (shipment == null) return false;

        foreach (var resource in shipment.ShipmentResources)
        {
            var balance = await _balanceRepo.Query()
                .FirstOrDefaultAsync(b => b.ResourceId == resource.ResourceId && b.UnitOfMeasureId == resource.UnitOfMeasureId);

            if (balance != null)
            {
                balance.Quantity += resource.Quantity;
                _balanceRepo.Update(balance);
            }
        }

        await _balanceRepo.SaveChangesAsync();
        return true;
    }
}