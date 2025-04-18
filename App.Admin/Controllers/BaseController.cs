using App.Shared.Services.File;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace App.Admin.Controllers;

public class BaseController(IFileService fileService) : Controller
{
    [HttpGet("GetImage")]
    public async Task<IActionResult> GetImage(string fileUrl)
    {
        var file = await fileService.GetFileAsync(fileUrl);
        var contentType = GetContentType(fileUrl);
        return File(file, contentType);
    }
    private static string GetContentType(string path)
    {
        var provider = new FileExtensionContentTypeProvider();
        return provider.TryGetContentType(path, out var contentType)
            ? contentType
            : "application/octet-stream";
    }

}
