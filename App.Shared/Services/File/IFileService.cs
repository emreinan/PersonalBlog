using App.Shared.Dto.File;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;

namespace App.Shared.Services.File
{
    public interface IFileService
    {
        Task<Result<string>> UploadFileAsync(IFormFile file);
        Task<Stream> GetDownloadFileAsync(string fileUrl);
        Task<Result> DeleteFileAsync(string fileUrl);
        Task<Stream> GetFileAsync(string fileUrl);
    }
}
