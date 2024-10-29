using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace App.Shared.Models;

public class ProjectSaveViewModel
{
    [Required, MinLength(5), MaxLength(50)]
    public string Title { get; set; }
    [Required, MinLength(5), MaxLength(500)]
    public string Description { get; set; }
    [Required]
    public IFormFile Image { get; set; }
    public bool IsActive { get; set; }

}


