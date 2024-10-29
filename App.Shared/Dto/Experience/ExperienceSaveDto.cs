using FluentValidation;

namespace App.Shared.Dto.Experience;

public class ExperienceSaveDto
{
    public string Title { get; set; }
    public string Company { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
}
public class ExperienceSaveDtoValidator : AbstractValidator<ExperienceSaveDto>
{
    public ExperienceSaveDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Company).NotEmpty();
        RuleFor(x => x.StartDate).NotEmpty();
    }
}
