using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

public class Register : AccountBase
{

    [Required]
    [MinLength(5)]
    [MaxLength(100)]
    public string? Fullname { get; set; }


    [DataType(DataType.Password)]
    [Required]
    [Compare(nameof(Password))]
    public string? ConfirmPassword { get; set; }
}

