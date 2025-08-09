namespace Domain.Entities;

public class ShipmentResource
{
    public Guid Id { get; set; }
    public Guid ShipmentDocumentId { get; set; }
    public Guid ResourceId { get; set; }
    public Guid UnitOfMeasureId { get; set; }
    public decimal Quantity { get; set; }
    public int AvailabilityNumber { get; set; } 

    public ShipmentDocument? ShipmentDocument { get; set; }
    public Resource? Resource { get; set; }
    public UnitOfMeasure? UnitOfMeasure { get; set; }
}
