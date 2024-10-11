using App.Shared.Dto.File;
using App.Shared.Services.Abstract;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.File.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController(IFileService fileService) : ControllerBase
    {

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file.");
            }
            var result = await fileService.UploadFileAsync( new FileUploadRequest
            {
                Name = file.FileName,
                Stream = file.OpenReadStream()
            });

            if (!result.IsSuccess)
            {
                return result.ToActionResult(this).Result ?? BadRequest("File cannot upload.");
            }
            return CreatedAtAction(nameof(Download), new { fileName = result.Value.FileName }, result.Value);
        }

        [HttpGet("Download")]
        public async Task<IActionResult> Download([FromQuery] string fileName )
        {
            var result = await fileService.DownloadFileAsync(new FileDownloadRequest { FileName = fileName });

            if (!result.IsSuccess)
            {
                return result.ToActionResult(this).Result ?? NotFound("File not found.");
            }
            var file = result.Value;

            return File(file.FileContent, file.ContentType, file.FileName);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery]string fileName)
        {
            var result = await fileService.DeleteFileAsync(fileName);
            return this.ToActionResult(result); // Sonuç ne ise ona göre dönüş yapar.
        }
    }
}
