using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ReceiptService : GenericService<ReceiptDocument>, IReceiptService
{
    private readonly IGenericRepository<ReceiptDocument> _receiptRepo;
    private readonly IGenericRepository<Balance> _balanceRepo;

    public ReceiptService(IGenericRepository<ReceiptDocument> receiptRepo,
                          IGenericRepository<Balance> balanceRepo)
        : base(receiptRepo)
    {
        _receiptRepo = receiptRepo;
        _balanceRepo = balanceRepo;
    }

    public async Task<bool> ProcessReceiptAsync(Guid receiptId)
    {
        var receipt = await _receiptRepo.GetByIdAsync(receiptId);
        if (receipt == null) return false;

        foreach (var resource in receipt.Resources)
        {
            var balance = await _balanceRepo.Query().FirstOrDefaultAsync(b => b.ResourceId == resource.ResourceId && b.UnitOfMeasureId == resource.UnitOfMeasureId);

            if (balance != null)
            {
                balance.Quantity += resource.Quantity;
                _balanceRepo.Update(balance);
            }
            else
            {
                await _balanceRepo.AddAsync(new Balance
                {
                    Id = Guid.NewGuid(),
                    ResourceId = resource.ResourceId,
                    UnitOfMeasureId = resource.UnitOfMeasureId,
                    Quantity = resource.Quantity,
                    IsActive = true
                });
            }
        }

        await _balanceRepo.SaveChangesAsync();
        return true;
    }
}
