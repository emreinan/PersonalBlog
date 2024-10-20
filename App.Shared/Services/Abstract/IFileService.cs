using App.Shared.Dto.File;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;

namespace App.Shared.Services.Abstract
{
    public interface IFileService
    {
        Task<Result<string>> UploadFileAsync(IFormFile file);
        Task<Result> DownloadFileAsync(string fileName);
        Task<Result> DeleteFileAsync(string fileName);
    }
}
