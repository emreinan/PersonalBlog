using App.Shared.Dto.AboutMe;
using App.Shared.Models;
using App.Shared.Services.AboutMe;
using App.Shared.Services.File;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers;

[Route("AboutMe")]
public class AboutMeController(IAboutMeService aboutMeService,IFileService fileService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var aboutMe = await aboutMeService.GetAboutMeAsync();
        return View(aboutMe);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("Edit")]
    public async Task<IActionResult> Edit()
    {
        var aboutMe = await aboutMeService.GetAboutMeAsync();
        var aboutMeUpdateViewModel = new AboutMeSaveViewModel
        {Introduciton = aboutMe.Introduciton,Title = aboutMe.Title};

        return View(aboutMeUpdateViewModel);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("Edit")]
    public async Task<IActionResult> Edit(AboutMeSaveViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var aboutMeDto = new AboutMeDto{Introduciton = model.Introduciton,Title = model.Title,Cv = model.Cv,Image1 = model.Image1,Image2 = model.Image2};
        await aboutMeService.UpdateAboutMeAsync(aboutMeDto);

        TempData["SuccessMessage"] = "About Me section updated successfully!";
        return RedirectToAction("Index");
    }

    [HttpGet("DownloadCv")]
    public async Task<IActionResult> DownloadCv()
    {
        var aboutMe = await aboutMeService.GetAboutMeAsync();
        if(aboutMe.Cv == null)
            return NotFound("Cv not found.");

        var file = await fileService.GetDownloadFileAsync(aboutMe.Cv);

        return File(file, "application/pdf", "Emre-Inan-Eng-Cv.pdf");
    }
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
