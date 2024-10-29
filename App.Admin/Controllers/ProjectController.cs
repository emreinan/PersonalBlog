using App.Shared.Dto.Project;
using App.Shared.Models;
using App.Shared.Services.Project;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers;
[Route("Project")]
public class ProjectController(IProjectService projectService, IMapper mapper) : Controller
{
    [HttpGet("Projects")]
    public async Task<IActionResult> Projects()
    {
        var projects = await projectService.GetProjects();
        return View(projects);
    }

    [HttpGet("Add")]
    public IActionResult Add()
    {
        return View();
    }
    [HttpPost("Add")]
    public async Task<IActionResult> Add(ProjectViewModel projectViewModel)
    {
        if (!ModelState.IsValid)
            return View(projectViewModel);

        var projectDto = mapper.Map<ProjectAddDto>(projectViewModel);
        await projectService.AddProjectAsync(projectDto);

        TempData["SuccessMessage"] = "Project added successfully";
        return RedirectToAction(nameof(Projects));
    }

    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var project = await projectService.GetProjectById(id);

        if (project is null)
            return NotFound();

        var projectViewModel = mapper.Map<ProjectViewModel>(project);
        return View(projectViewModel);
    }

    [HttpPost("Edit/{id}")]
    public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] ProjectViewModel projectViewModel)
    {
        if (!ModelState.IsValid)
            return View(projectViewModel);

        var projectDto = mapper.Map<ProjectEditDto>(projectViewModel);
        await projectService.EditProjectAsync(id, projectDto);

        TempData["SuccessMessage"] = "Project updated successfully";
        return RedirectToAction(nameof(Projects));
    }

    [HttpGet("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await projectService.GetProjectById(id);
        await projectService.DeleteProjectAsync(id);

        TempData["SuccessMessage"] = "Project deleted successfully";
        return RedirectToAction(nameof(Projects));
    }

    [HttpGet("MakeActive/{id}")]
    public async Task<IActionResult> MakeActive(int id)
    {
        await projectService.MakeActiveProject(id);

        TempData["SuccessMessage"] = "Project is now active";
        return RedirectToAction(nameof(Projects));
    }

    [HttpGet("MakeInActive/{id}")]
    public async Task<IActionResult> Hide(int id)
    {
        await projectService.MakeInActiveProject(id);

        TempData["SuccessMessage"] = "Project is now inactive";
        return RedirectToAction(nameof(Projects));
    }
}
