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
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file.");
            }
            var filePath = Path.Combine(GetFileSaveFolder(), file.FileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok(file.FileName);
            }
            catch (IOException)
            {
                return Conflict("File already exists.");
            }
        }

        [HttpGet("Download")]
        public async Task<IActionResult> Download([FromQuery] string fileName)
        {
            var filePath = Path.Combine(GetFileSaveFolder(), fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            var contentType = GetContentType(filePath);

            return File(fileBytes, contentType, fileName);

        }

        [HttpDelete("Delete")]
        public IActionResult Delete([FromQuery] string fileName)
        {

            var filePath = Path.Combine(GetFileSaveFolder(), fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            System.IO.File.Delete(filePath);

            return Ok(filePath);
        }

        [HttpGet("GetByUrl")]
        public IActionResult GetByUrl([FromQuery] string fileName)
        {
            var filePath = Path.Combine(GetFileSaveFolder(), fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }
            var contentType =GetContentType(filePath);
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, contentType, fileName);
        }
        private static string GetFileSaveFolder()
        {
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            // wwwroot klasörü yoksa oluşturuyoruz
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            var uploadFolderPath = Path.Combine(rootPath, "uploads");

            if (!Directory.Exists(uploadFolderPath))
                Directory.CreateDirectory(uploadFolderPath);

            return uploadFolderPath; // Dosyaların kaydedileceği klasör
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
