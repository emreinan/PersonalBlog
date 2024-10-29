using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace App.Shared.Models;

public class AboutMeSaveViewModel
{
    [Required, MinLength(10)]
    public string Introduciton { get; set; }
    [Required, MinLength(5)]
    public string Title { get; set; }
    public IFormFile? Cv { get; set; }
    public IFormFile? Image1 { get; set; }
    public IFormFile? Image2 { get; set; }
}
