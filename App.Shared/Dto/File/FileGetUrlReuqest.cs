using FluentValidation;

namespace App.Shared.Dto.File;

public class FileGetUrlReuqest
{
    public required string FileName { get; set; }
}
public class FileGetUrlReuqestValidator : AbstractValidator<FileGetUrlReuqest>
{
    public FileGetUrlReuqestValidator()
    {
        RuleFor(x => x.FileName).NotEmpty();
    }
}
