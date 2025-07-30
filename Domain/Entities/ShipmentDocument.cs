namespace Domain.Entities;

public class ShipmentDocument
{
    public Guid Id { get; set; }
    public string Number { get; set; } = null!;
    public Guid ClientId { get; set; }
    public DateTime Date { get; set; }
    public bool IsSigned { get; set; } = false;

    public Client Client { get; set; } = null!;
    public ICollection<ShipmentResource> ShipmentResources { get; set; } = new List<ShipmentResource>();

}
