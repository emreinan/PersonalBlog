﻿using FluentValidation;

namespace App.Shared.Dto.File
{
    public class FileDownloadRequest
    {
        public string FileName { get; set; } = null!;
    }

    public class FileDownloadRequestValidator : AbstractValidator<FileDownloadRequest>
    {
        public FileDownloadRequestValidator()
        {
            RuleFor(x => x.FileName).NotEmpty()
                .MinimumLength(1)
                .Must(x => x.Contains('.'));
        }
    }
}