using Domain.Entities;

namespace Application.Interfaces
{
    public interface IReceiptResourceService : IGenericService<ReceiptResource>
    {
        Task<bool> UpdateResourceQuantityAsync(Guid receiptResourceId, decimal newQuantity);
    }
}
