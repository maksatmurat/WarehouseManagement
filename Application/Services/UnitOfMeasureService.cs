using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;
public class UnitOfMeasureService : GenericService<UnitOfMeasure>, IUnitOfMeasureService
{
    private readonly IGenericRepository<UnitOfMeasure> _unitRepo;
    private readonly IGenericRepository<Balance> _balanceRepo;
    private readonly IGenericRepository<ReceiptResource> _receiptResRepo;
    private readonly IGenericRepository<ShipmentResource> _shipmentResRepo;

    public UnitOfMeasureService(
        IGenericRepository<UnitOfMeasure> unitRepo,
        IGenericRepository<Balance> balanceRepo,
        IGenericRepository<ReceiptResource> receiptResRepo,
        IGenericRepository<ShipmentResource> shipmentResRepo
    ) : base(unitRepo)
    {
        _unitRepo = unitRepo;
        _balanceRepo = balanceRepo;
        _receiptResRepo = receiptResRepo;
        _shipmentResRepo = shipmentResRepo;
    }

    public override async Task<UnitOfMeasure> CreateAsync(UnitOfMeasure entity)
    {
        bool exists = await _unitRepo.ExistsAsync(u => u.Name == entity.Name && u.IsActive);
        if (exists)
            throw new InvalidOperationException("Единица измерения с таким именем уже существует.");

        return await base.CreateAsync(entity);
    }

    public override async Task<UnitOfMeasure> UpdateAsync(Guid id, UnitOfMeasure entity)
    {
        bool exists = await _unitRepo.ExistsAsync(u => u.Id != id && u.Name == entity.Name && u.IsActive);
        if (exists)
            throw new InvalidOperationException("Единица измерения с таким именем уже существует.");

        return await base.UpdateAsync(id, entity);
    }

    public override async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _unitRepo.GetByIdAsync(id);
        if (entity == null) return false;

        bool isUsed =
            await _balanceRepo.ExistsAsync(b => b.UnitOfMeasureId == id) ||
            await _receiptResRepo.ExistsAsync(rr => rr.UnitOfMeasureId == id) ||
            await _shipmentResRepo.ExistsAsync(sr => sr.UnitOfMeasureId == id);

        if (isUsed)
        {
            entity.IsActive = false;
            _unitRepo.Update(entity);
        }
        else
        {
            _unitRepo.Remove(entity);
        }

        await _unitRepo.SaveChangesAsync();
        return true;
    }

}
