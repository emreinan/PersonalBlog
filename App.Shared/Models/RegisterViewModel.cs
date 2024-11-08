﻿using System.ComponentModel.DataAnnotations;

namespace App.Shared.Models;

public class RegisterViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = null!;
    [Required, MinLength(4), DataType(DataType.Password)]
    public string Password { get; set; } = null!;
    [Required, MinLength(1), MaxLength(50)]
    public string UserName { get; set; } = null!;
}