
using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ReceiptDocumentService : GenericService<ReceiptDocument>, IReceiptDocumentService
{
    private readonly IGenericRepository<Balance> _balanceRepo;
    private readonly IGenericRepository<ReceiptResource> _receiptResourceRepo;

    public ReceiptDocumentService(
        IGenericRepository<ReceiptDocument> repository,
        IGenericRepository<Balance> balanceRepo,
        IGenericRepository<ReceiptResource> receiptResourceRepo
    ) : base(repository)
    {
        _balanceRepo = balanceRepo;
        _receiptResourceRepo = receiptResourceRepo;
    }

    public async Task<bool> SignDocumentAsync(Guid documentId)
    {
        var resources = await _receiptResourceRepo.Query()
            .Where(r => r.ReceiptDocumentId == documentId).ToListAsync();

        foreach (var r in resources)
        {
            var balance = await _balanceRepo.Query()
                .FirstOrDefaultAsync(b => b.ResourceId == r.ResourceId && b.UnitOfMeasureId == r.UnitOfMeasureId);

            if (balance == null)
            {
                balance = new Balance
                {
                    Id = Guid.NewGuid(),
                    ResourceId = r.ResourceId,
                    UnitOfMeasureId = r.UnitOfMeasureId,
                    Quantity = r.Quantity
                };
                await _balanceRepo.AddAsync(balance);
            }
            else
            {
                balance.Quantity += r.Quantity;
                _balanceRepo.Update(balance);
            }
        }

        await _balanceRepo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CancelDocumentAsync(Guid documentId)
    {
        var resources = await _receiptResourceRepo.Query()
            .Where(r => r.ReceiptDocumentId == documentId).ToListAsync();

        foreach (var r in resources)
        {
            var balance = await _balanceRepo.Query()
                .FirstOrDefaultAsync(b => b.ResourceId == r.ResourceId && b.UnitOfMeasureId == r.UnitOfMeasureId);

            if (balance == null || balance.Quantity < r.Quantity)
                return false; // Недостаточно ресурсов

            balance.Quantity -= r.Quantity;
            _balanceRepo.Update(balance);
        }

        await _balanceRepo.SaveChangesAsync();
        return true;
    }
}
