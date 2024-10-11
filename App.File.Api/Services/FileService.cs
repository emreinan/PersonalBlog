using App.Shared.Dto.File;
using App.Shared.Services.Abstract;
using Ardalis.Result;

namespace App.File.Api.Services;

public class FileService : IFileService
{

    public async Task<Result> DeleteFileAsync(string fileName)
    {
        var filePath = Path.Combine(GetFileSaveFolder(), fileName);

        if (!System.IO.File.Exists(filePath))
            return Result.NotFound("File not found.");

        System.IO.File.Delete(filePath);

        return Result.Success();
    }

    public async Task<Result<FileDownloadResponse>> DownloadFileAsync(FileDownloadRequest fileDownloadRequest)
    {
        var filePath = Path.Combine(GetFileSaveFolder(), fileDownloadRequest.FileName);

        if(!System.IO.File.Exists(filePath))
            return Result<FileDownloadResponse>.NotFound("File not found.");

        var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
        var contentType = GetContentType(filePath);

        return Result<FileDownloadResponse>.Success(new FileDownloadResponse
        {
            FileName = fileDownloadRequest.FileName,
            ContentType = contentType,
            FileContent = fileBytes
        });
    }

    public async Task<Result<FileUploadResponse>> UploadFileAsync(FileUploadRequest uploadFileRequest)
    {
        var filePath = Path.Combine(GetFileSaveFolder(), uploadFileRequest.Name);

        try
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
               await uploadFileRequest.Stream.CopyToAsync(stream);
            }

            return Result<FileUploadResponse>.Success(new FileUploadResponse { FileName = uploadFileRequest.Name });
        }
        catch (IOException)
        {
            return Result.Conflict("File already exists.");
        }

    }
    private static string GetFileSaveFolder()
    {
        var fileSaveFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        if (!Directory.Exists(fileSaveFolder))
        {
            Directory.CreateDirectory(fileSaveFolder);
        }

        return fileSaveFolder;
    }
    private string GetContentType(string path)
    {
        var types = new Dictionary<string, string>
        {
            {".jpg", "image/jpeg"},
            {".png", "image/png"},
            {".txt", "text/plain"},
            {".pdf", "application/pdf"}
        };

        var ext = Path.GetExtension(path).ToLowerInvariant();
        return types.ContainsKey(ext) ? types[ext] : "application/octet-stream";
    }
}
