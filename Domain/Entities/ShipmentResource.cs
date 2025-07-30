namespace Domain.Entities;

public class ShipmentResource
{
    public Guid Id { get; set; }
    public Guid ShipmentDocumentId { get; set; }
    public Guid ResourceId { get; set; }
    public Guid UnitOfMeasureId { get; set; }
    public decimal Quantity { get; set; }

    public ShipmentDocument ShipmentDocument { get; set; } = null!;
    public Resource Resource { get; set; } = null!;
    public UnitOfMeasure UnitOfMeasure { get; set; } = null!;
}
