using App.File.Api.Services;
using App.Shared.Dto.File;
using App.Shared.Services.Abstract;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;


namespace App.File.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(FileUploadRequest fileUploadRequest)
        {
            if (fileUploadRequest.File == null || fileUploadRequest.File.Length == 0)
            {
                return BadRequest("Invalid file.");
            }
            var filePath = Path.Combine(GetFileSaveFolder(), fileUploadRequest.File.FileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await fileUploadRequest.File.CopyToAsync(stream);
                }

                return Ok(fileUploadRequest.File.FileName);
            }
            catch (IOException)
            {
                return Conflict("File already exists.");
            }
        }

        [HttpGet("Download")]
        public async Task<IActionResult> Download([FromQuery] FileDownloadRequest fileDownloadRequest)
        {
            var filePath = Path.Combine(GetFileSaveFolder(), fileDownloadRequest.FileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            var contentType = GetContentType(filePath);

            return File(fileBytes, contentType, fileDownloadRequest.FileName);

        }

        [HttpDelete("Delete")]
        public IActionResult Delete([FromQuery] FileDeleteRequest fileDeleteRequest)
        {
            var filePath = Path.Combine(GetFileSaveFolder(), fileDeleteRequest.FileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            System.IO.File.Delete(filePath);

            return Ok(filePath);
        }

        [HttpGet("GetByUrl")]
        public IActionResult GetByUrl([FromQuery] FileGetUrlReuqest fileGetUrlReuqest)
        {
            var filePath = Path.Combine(GetFileSaveFolder(), fileGetUrlReuqest.FileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }
            var contentType =GetContentType(filePath);
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, contentType, fileGetUrlReuqest.FileName);
        }
        private static string GetFileSaveFolder()
        {
            var uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

            // uploads klasörü yoksa oluşturuyoruz
            if (!Directory.Exists(uploadFolderPath))
                Directory.CreateDirectory(uploadFolderPath);

            return uploadFolderPath; 
        }
        private static string GetContentType(string path)
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
}
