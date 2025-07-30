namespace Domain.Entities;

public class Balance
{
    public Guid Id { get; set; }
    public Guid ResourceId { get; set; }
    public Guid UnitOfMeasureId { get; set; }
    public decimal Quantity { get; set; }

    public Resource Resource { get; set; } = null!;
    public UnitOfMeasure UnitOfMeasure { get; set; } = null!;
}
