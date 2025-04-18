using FluentValidation;

namespace App.Shared.Dto.File;

public class FileDeleteRequest
{
    public required string FileUrl { get; set; }
}
public class FileDeleteRequestValidator : AbstractValidator<FileDeleteRequest>
{
    public FileDeleteRequestValidator()
    {
        RuleFor(x => x.FileUrl).NotEmpty();
    }
}