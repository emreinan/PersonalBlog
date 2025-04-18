using App.Shared.Dto.File;
using App.Shared.Util.ExceptionHandling.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;


namespace App.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private const int MaxFileSize = 2 * 1024 * 1024;
        private const string UploadFolderName = "uploads";

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload([FromForm] FileUploadRequest fileUploadRequest)
        {
            var file = fileUploadRequest.File;
            var filePath = Path.Combine(GetFileSaveFolder(), file.FileName);

            if (System.IO.File.Exists(filePath))
                throw new ConflictException("File already exists.");

            if (file.Length > MaxFileSize)
                throw new ValidationException($"Dosya boyutu {MaxFileSize / 1024}KB'ı geçemez. Yüklediğiniz dosya: {file.Length / 1024}KB");

            await using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream);

            var fileUrl = GetFileUrl(file.FileName);

            return Ok(fileUrl);

        }

        [HttpGet("Download")]
        public IActionResult Download([FromQuery] FileDownloadRequest fileDownloadRequest)
        {
            var fileName = Path.GetFileName(fileDownloadRequest.FileUrl);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", UploadFolderName, fileName);

            if (!System.IO.File.Exists(filePath))
                throw new NotFoundException("File not found.");

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var contentType = GetContentType(filePath);

            return File(fileBytes, contentType, fileName);
        }

        [HttpGet("GetImage")]
        public IActionResult GetImage([FromQuery] string fileUrl)
        {
            var fileName = Path.GetFileName(fileUrl);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", UploadFolderName, fileName);

            if (!System.IO.File.Exists(filePath))
                throw new NotFoundException("File not found.");

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var contentType = GetContentType(filePath);

            return File(fileBytes, contentType);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete([FromQuery] FileDeleteRequest fileDeleteRequest)
        {
            var fileName = Path.GetFileName(fileDeleteRequest.FileUrl);
            var filePath = Path.Combine(GetFileSaveFolder(), fileName);

            if (!System.IO.File.Exists(filePath))
                throw new NotFoundException("File not found.");

            System.IO.File.Delete(filePath);

            return Ok(filePath);
        }

        private string GetFileUrl(string fileName)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var fileUrl = $"{baseUrl}/images/{fileName}";

            return fileUrl;
        }
        private static string GetFileSaveFolder()
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", UploadFolderName);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            return folderPath;
        }

        private static string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            return provider.TryGetContentType(path, out var contentType)
                ? contentType
                : "application/octet-stream";
        }
    }
}
