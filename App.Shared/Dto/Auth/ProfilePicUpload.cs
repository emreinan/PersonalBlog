using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace App.Shared.Dto.Auth;

public class ProfilePicUpload
{
    public required IFormFile File { get; set; }
}
public class ProfilePicUploadValidator : AbstractValidator<ProfilePicUpload>
{
    public ProfilePicUploadValidator()
    {
        RuleFor(x => x.File).NotNull().WithMessage("File is required.");
    }
}
