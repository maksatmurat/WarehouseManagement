namespace Application.DTOs;

public class ShipmentFilterDto
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? Number { get; set; }
    public Guid? ClientId { get; set; }
    public Guid? ResourceId { get; set; }
    public Guid? UnitOfMeasureId { get; set; }
}
