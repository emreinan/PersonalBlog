using System.ComponentModel.DataAnnotations;

namespace App.Shared.Models;

public class ForgotPasswordViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; }
}
