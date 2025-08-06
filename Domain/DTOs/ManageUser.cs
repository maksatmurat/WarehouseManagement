namespace Domain.DTOs;

public class ManageUser
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public Guid UserId { get; set; }
    public string? Role { get; set; }
}
