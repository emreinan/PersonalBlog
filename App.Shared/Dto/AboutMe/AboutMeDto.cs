using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace App.Shared.Dto.AboutMe;

public class AboutMeDto
{
    public required string Introduciton { get; set; } 
    public required string Title { get; set; } 
    public IFormFile? Cv { get; set; }
    public IFormFile? Image1 { get; set; }
    public IFormFile? Image2 { get; set; }
}
public class AboutMeDtoValidator : AbstractValidator<AboutMeDto>
{
    public AboutMeDtoValidator()
    {
        RuleFor(x => x.Introduciton).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
    }
}
