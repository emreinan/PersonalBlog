﻿using App.Shared.Dto.File;
using Microsoft.AspNetCore.Mvc;


namespace App.File.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload([FromForm] FileUploadRequest fileUploadRequest)
        {
            var file = fileUploadRequest.File;
            var filePath = Path.Combine(GetFileSaveFolder(), file.FileName);

            if (System.IO.File.Exists(filePath))
            {
                return Conflict("File already exists.");
            }

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                var fileUrl = GetFileUrl(file.FileName);

                return Ok(fileUrl);
            }
            catch (IOException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet("Download")]
        public IActionResult Download([FromQuery] FileDownloadRequest fileDownloadRequest)
        {
            var fileName = Path.GetFileName(fileDownloadRequest.FileUrl);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var contentType = GetContentType(filePath);

            return File(fileBytes, contentType, fileName);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete([FromQuery] FileDeleteRequest fileDeleteRequest)
        {
            var fileName = Path.GetFileName(fileDeleteRequest.FileUrl);
            var filePath = Path.Combine(GetFileSaveFolder(), fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

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
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            return folderPath;
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
