using System.ComponentModel.DataAnnotations;

namespace App.Client.Models;

public class RegisterViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = null!;

    [Required, MinLength(4), DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required, Compare(nameof(Password)), DataType(DataType.Password)]
    public string PasswordRepeat { get; set; } = null!;

    [Required, MinLength(1), MaxLength(50)]
    public string Nickname { get; set; } = null!;
}