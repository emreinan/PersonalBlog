using FluentValidation;

namespace App.Shared.Dto.Education
{
    public class EducationSaveDto
    {
        public required string School { get; set; }
        public required string Degree { get; set; }
        public required string FieldOfStudy { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public class EducationSaveDtoValidator : AbstractValidator<EducationSaveDto>
    {
        public EducationSaveDtoValidator()
        {

            RuleFor(x => x.School).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Degree).NotEmpty().MaximumLength(100);
            RuleFor(x => x.FieldOfStudy).NotEmpty().MaximumLength(100);
            RuleFor(x => x.StartDate).NotEmpty();
        }
    }
}



