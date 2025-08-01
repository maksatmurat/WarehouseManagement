namespace Application.DTOs;

public class UserSession
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
}
