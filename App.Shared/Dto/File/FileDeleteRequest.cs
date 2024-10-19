using FluentValidation;

namespace App.Shared.Dto.File;

public class FileDeleteRequest
{
    public string FileName { get; set; } 
}
public class FileDeleteRequestValidator : AbstractValidator<FileDeleteRequest>
{
    public FileDeleteRequestValidator()
    {
        RuleFor(x => x.FileName).NotEmpty()
            .MinimumLength(1)
            .Must(x => x.Contains('.'));
    }
}
