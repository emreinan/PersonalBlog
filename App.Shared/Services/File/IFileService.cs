using Microsoft.AspNetCore.Http;

namespace App.Shared.Services.File
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file);
        Task<Stream> GetDownloadFileAsync(string fileUrl);
        Task DeleteFileAsync(string fileUrl);
        Task<Stream> GetFileAsync(string fileUrl);
    }
}
