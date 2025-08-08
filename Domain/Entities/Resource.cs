using Domain.DTOs;

namespace Domain.Entities;

public class Resource:IHasName
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; } = true;

    public ICollection<Balance> Balances { get; set; } = new List<Balance>();
    public ICollection<ReceiptResource> ReceiptResources { get; set; } = new List<ReceiptResource>();
    public ICollection<ShipmentResource> ShipmentResources { get; set; } = new List<ShipmentResource>();

}
