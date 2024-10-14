using FluentValidation;
using Microsoft.AspNetCore.Http;


namespace App.Shared.Dto.AboutMe;

public class AboutMeUpdateDto
{
    public string Introduciton { get; set; }
    public IFormFile? ImageUrl1 { get; set; }
    public IFormFile? ImageUrl2 { get; set; }
}

public class AboutMeUpdateDtoValidator : AbstractValidator<AboutMeUpdateDto>
{
    public AboutMeUpdateDtoValidator()
    {
        RuleFor(x => x.Introduciton).NotEmpty();
    }
}
