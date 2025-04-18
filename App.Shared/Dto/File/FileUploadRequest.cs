using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace App.Shared.Dto.File;

public class FileUploadRequest
{
    public required IFormFile File { get; set; }
}
public class FileUploadRequestValidator : AbstractValidator<FileUploadRequest>
{
    public FileUploadRequestValidator()
    {
        RuleFor(x => x.File).NotNull().WithMessage("File is not uploaded.")
            .Must(x=>x.Length>0).WithMessage("File can not be empty.");
    }
}