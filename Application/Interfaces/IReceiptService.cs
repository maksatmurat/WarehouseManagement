using Domain.Entities;

namespace Application.Interfaces;

public interface IReceiptService : IGenericService<ReceiptDocument>
{
    Task<bool> ProcessReceiptAsync(Guid receiptId);
}
