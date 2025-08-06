
using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

public class AccountBase
{
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    [Required]
    public string? Email { get; set; }

    [DataType(DataType.Password)]
    [Required]
    public string? Password
    { get; set; }
}
