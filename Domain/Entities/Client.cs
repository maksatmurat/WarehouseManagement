namespace Domain.Entities;

public class Client
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public bool IsActive { get; set; } = true;

    public ICollection<ShipmentDocument> Shipments { get; set; } = new List<ShipmentDocument>();

}
