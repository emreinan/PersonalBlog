using System.ComponentModel.DataAnnotations;

namespace App.Client.Models;

public class UserUpdateViewModel
{
    public Guid Id { get; set; }
    [Required, MinLength(2)]
    public string UserName { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; }

    public string ProfilePhoto { get; set; }

}
