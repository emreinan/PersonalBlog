
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace App.Shared.Dto.Project;

public class ProjectAddDto
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required IFormFile Image { get; set; }
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
