using App.Shared.Services.File;
using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers;

public class BaseController(IFileService fileService) : Controller
{
    [HttpGet("GetImage")]
    public async Task<IActionResult> GetImage(string fileUrl)
    {
        try
        {
            var file = await fileService.GetFileAsync(fileUrl);
            var contentType = GetContentType(fileUrl);
            return File(file, contentType);
        }
        catch (HttpRequestException)
        {
            return NotFound("File not found.");
        }
    }
    private static string GetContentType(string fileUrl)
    {
        var types = new Dictionary<string, string>
        {
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".png", "image/png" },
            { ".gif", "image/gif" },
            {".txt", "text/plain"},
            {".pdf", "application/pdf"}
        };

        var ext = Path.GetExtension(fileUrl).ToLowerInvariant();
        return types.ContainsKey(ext) ? types[ext] : "application/octet-stream";
    }

}
