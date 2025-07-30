namespace Domain.Entities;

public class ReceiptDocument
{
    public Guid Id { get; set; }
    public string Number { get; set; } = null!;
    public DateTime Date { get; set; }

    public ICollection<ReceiptResource> Resources { get; set; } = new List<ReceiptResource>();

}
