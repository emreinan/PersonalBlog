
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace App.Shared.Dto.Project;

public class ProjectEditDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public IFormFile? Image { get; set; }
}
public class ProjectEditDtoValidator : AbstractValidator<ProjectEditDto>
{
    public ProjectEditDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
    }
}

