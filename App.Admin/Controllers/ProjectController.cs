using App.Shared.Dto.Project;
using App.Shared.Models;
using App.Shared.Services.File;
using App.Shared.Services.Project;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers;
[Route("Project")]
public class ProjectController(IProjectService projectService, IMapper mapper,IFileService fileService) : Controller
{
    [HttpGet("Projects")]
    public async Task<IActionResult> Projects()
    {
        var projects = await projectService.GetProjectsAsync();
        return View(projects);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("Add")]
    public IActionResult Add()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("Add")]
    public async Task<IActionResult> Add(ProjectAddViewModel projectAddViewModel)
    {
        if (!ModelState.IsValid)
            return View(projectAddViewModel);

        var projectDto = mapper.Map<ProjectAddDto>(projectAddViewModel);
        await projectService.AddProjectAsync(projectDto);

        TempData["SuccessMessage"] = "Project added successfully";
        return RedirectToAction(nameof(Projects));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var project = await projectService.GetProjectByIdAsync(id);

        if (project is null)
            return NotFound();

        var projectViewModel = mapper.Map<ProjectEditViewModel>(project);
        return View(projectViewModel);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("Edit/{id}")]
    public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] ProjectEditViewModel projectEditViewModel)
    {
        if (!ModelState.IsValid)
            return View(projectEditViewModel);

        var projectDto = mapper.Map<ProjectEditDto>(projectEditViewModel);
        await projectService.EditProjectAsync(id, projectDto);

        TempData["SuccessMessage"] = "Project updated successfully";
        return RedirectToAction(nameof(Projects));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await projectService.GetProjectByIdAsync(id);
        await projectService.DeleteProjectAsync(id);

        TempData["SuccessMessage"] = "Project deleted successfully";
        return RedirectToAction(nameof(Projects));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("MakeActive/{id}")]
    public async Task<IActionResult> MakeActive(int id)
    {
        await projectService.MakeActiveProjectAsync(id);

        TempData["SuccessMessage"] = "Project is now active";
        return RedirectToAction(nameof(Projects));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("MakeInActive/{id}")]
    public async Task<IActionResult> Hide(int id)
    {
        await projectService.MakeInActiveProjectAsync(id);

        TempData["SuccessMessage"] = "Project is now inactive";
        return RedirectToAction(nameof(Projects));
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
