using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace App.Shared.Models;

public class ProjectEditViewModel
{
    [Required, MinLength(5), MaxLength(50)]
    public string Title { get; set; }
    [Required, MinLength(5), MaxLength(500)]
    public string Description { get; set; }
    public IFormFile? Image { get; set; }
}


