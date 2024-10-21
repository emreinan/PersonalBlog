using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Shared.Dto.Auth;

public class ProfilePicUpload
{
    public IFormFile File { get; set; }
}
public class ProfilePicUploadValidator : AbstractValidator<ProfilePicUpload>
{
    public ProfilePicUploadValidator()
    {
        RuleFor(x => x.File).NotNull().WithMessage("File is required.");
    }
}
