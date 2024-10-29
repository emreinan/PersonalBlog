using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Shared.Models;

public class BlogPostCreatedViewModel
{
    [Required,MinLength(5), MaxLength(50)]
    public string Title { get; set; }
    [Required, MinLength(5), MaxLength(1000)]
    public string Content { get; set; }
    [Required, DataType(DataType.Upload)]
    public IFormFile Image { get; set; }
}
