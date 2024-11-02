using System.ComponentModel.DataAnnotations;

namespace App.Shared.Models
{
    public class ResetPasswordViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = default!;
        [Required, MinLength(1)]
        public string Password { get; set; } = default!;

        [Required, MinLength(1), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = default!;
    }
}