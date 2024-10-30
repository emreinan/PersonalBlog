using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace App.Shared.Models;

public class UserUpdateViewModel
{
    [Required, MinLength(2)]
    public string UserName { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; }
}
