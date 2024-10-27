using System.ComponentModel.DataAnnotations;

namespace App.Shared.Models;

public class ContactMessageViewModel
{
    public int Id { get; set; }
    [Required, MaxLength(50), MinLength(2)]
    public string Name { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required, MaxLength(50), MinLength(2)]
    public string Subject { get; set; }
    [Required, MaxLength(500), MinLength(2)]
    public string Message { get; set; }
    public bool IsRead { get; set; }
}

