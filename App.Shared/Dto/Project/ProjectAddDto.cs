
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace App.Shared.Dto.Project;

public class ProjectAddDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public IFormFile Image { get; set; }
}
public class ProjectAddDtoValidator : AbstractValidator<ProjectAddDto>
{
    public ProjectAddDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Image).NotEmpty();
    }
}
