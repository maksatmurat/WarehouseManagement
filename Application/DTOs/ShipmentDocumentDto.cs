namespace Application.DTOs;

public class ShipmentDocumentDto
{
    public Guid Id { get; set; }
    public string Number { get; set; } = null!;
    public DateTime Date { get; set; }
    public string ClientName { get; set; } = null!;
    public bool IsSigned { get; set; }
    public string ResourceName { get; set; } = null!;
    public string UnitOfMeasureName { get; set; } = null!;
    public decimal Quantity { get; set; }
    public int AvailabilityNumber { get; set; }
}
