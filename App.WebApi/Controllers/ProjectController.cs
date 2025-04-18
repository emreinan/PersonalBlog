using App.Data.Contexts;
using App.Data.Entities;
using App.Shared.Dto.Project;
using App.Shared.Services.File;
using App.Shared.Util.ExceptionHandling.Types;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController(AppDbContext context, IMapper mapper, IFileService fileService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var projects = await context.Projects.ToListAsync();
        var projectDtos = mapper.Map<List<ProjectDto>>(projects);

        return Ok(projectDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProject(int id)
    {
        var project = await context.Projects.FindAsync(id) ??
            throw new NotFoundException("Project not found");

        var projectDto = mapper.Map<ProjectDto>(project);
        return Ok(projectDto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateProject(ProjectAddDto projectAddDto)
    {
        var result = await fileService.UploadFileAsync(projectAddDto.Image);

        var project = new Project
        {
            Title = projectAddDto.Title,
            Description = projectAddDto.Description,
            ImageUrl = result,
            CreatedAt = DateTime.UtcNow
        };

        context.Projects.Add(project);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, projectAddDto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(int id, ProjectEditDto projectEditDto)
    {
        var project = await context.Projects.FindAsync(id)
                ?? throw new NotFoundException("Project not found.");

        if (projectEditDto.Image is not null)
        {
            var result = await fileService.UploadFileAsync(projectEditDto.Image);

            project.ImageUrl = result;
        }

        project.Title = projectEditDto.Title;
        project.Description = projectEditDto.Description;
        project.UpdatedAt = DateTime.Now;

        context.Projects.Update(project);
        await context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var project = await context.Projects.FindAsync(id) ??
            throw new NotFoundException("Project not found");

        context.Projects.Remove(project);
        await context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("Active/{id}")]
    public async Task<IActionResult> MakeActiveProject(int id)
    {
        var project = await context.Projects.FindAsync(id) ??
            throw new NotFoundException("Project not found");

        if (project.IsActive)
            throw new BadRequestException("Project is already active");

        project.IsActive = true;
        await context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("InActive/{id}")]
    public async Task<IActionResult> MakeInActiveProject(int id)
    {
        var project = await context.Projects.FindAsync(id) ??
            throw new NotFoundException("Project not found");

        if (!project.IsActive)
            throw new BadRequestException("Project is already inactive");

        project.IsActive = false;
        await context.SaveChangesAsync();

        return NoContent();
    }

}

