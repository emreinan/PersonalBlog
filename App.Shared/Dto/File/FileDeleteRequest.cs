using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Shared.Dto.File;

public class FileDeleteRequest
{
    public string FileUrl { get; set; }
}
public class FileDeleteRequestValidator : AbstractValidator<FileDeleteRequest>
{
    public FileDeleteRequestValidator()
    {
        RuleFor(x => x.FileUrl).NotEmpty();
    }
}