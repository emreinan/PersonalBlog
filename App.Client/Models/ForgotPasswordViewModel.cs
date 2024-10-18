using System.ComponentModel.DataAnnotations;

namespace App.Client.Models;

public class ForgotPasswordViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; }
}
