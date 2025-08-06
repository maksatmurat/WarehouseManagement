using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;

namespace Application.Services;

public class ReceiptResourceService : GenericService<ReceiptResource>, IReceiptResourceService
{
    public ReceiptResourceService(IGenericRepository<ReceiptResource> repository)
        : base(repository) { }

    public async Task<bool> UpdateResourceQuantityAsync(Guid receiptResourceId, decimal newQuantity)
    {
        var resource = await _repository.GetByIdAsync(receiptResourceId);
        if (resource == null) return false;

        resource.Quantity = newQuantity;
        _repository.Update(resource);
        await _repository.SaveChangesAsync();
        return true;
    }
}
