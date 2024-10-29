using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace App.Shared.Models;

public class BlogPostEditViewModel
{
    public Guid Id { get; set; }
    [Required, MinLength(5), MaxLength(50)]
    public string Title { get; set; }
    [Required, MinLength(5), MaxLength(1000)]
    public string Content { get; set; }
    [DataType(DataType.Upload)]
    public IFormFile? Image { get; set; }
}

