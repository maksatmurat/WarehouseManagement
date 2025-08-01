
using Domain.Entities;

namespace Application.Interfaces;

public interface IReceiptDocumentService : IGenericService<ReceiptDocument>
{
    Task<bool> SignDocumentAsync(Guid documentId);
    Task<bool> CancelDocumentAsync(Guid documentId);
}
