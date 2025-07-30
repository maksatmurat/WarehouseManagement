namespace Domain.Entities;

public class ReceiptResource
{
    public Guid Id { get; set; }
    public Guid ReceiptDocumentId { get; set; }
    public Guid ResourceId { get; set; }
    public Guid UnitOfMeasureId { get; set; }
    public decimal Quantity { get; set; }

    public ReceiptDocument ReceiptDocument { get; set; } = null!;
    public Resource Resource { get; set; } = null!;
    public UnitOfMeasure UnitOfMeasure { get; set; } = null!;
}
