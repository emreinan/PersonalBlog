using FluentValidation;

namespace App.Shared.Dto.File
{
    public class FileDownloadRequest
    {
        public string FileUrl { get; set; } = null!;
    }

    public class FileDownloadRequestValidator : AbstractValidator<FileDownloadRequest>
    {
        public FileDownloadRequestValidator()
        {
            RuleFor(x => x.FileUrl).NotEmpty()
                .MinimumLength(1)
                .Must(x => x.Contains('.'));
        }
    }
}