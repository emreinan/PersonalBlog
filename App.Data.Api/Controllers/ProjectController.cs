using App.Data.Contexts;
using App.Data.Entities.Data;
using App.Shared.Dto.Project;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController(DataDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var projects = await context.Projects.ToListAsync();
        return Ok(projects);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProject(int id)
    {
        var project = await context.Projects.FindAsync(id);

        if (project == null)
        {
            return NotFound();
        }

        return Ok(project);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(ProjectDto projectDto)
    {
        var project = new Project
        {
            Title = projectDto.Title,
            Description = projectDto.Description,
            ImageUrl = projectDto.ImageUrl,
            CreatedAt = DateTime.Now
        };

        context.Projects.Add(project);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(int id, ProjectDto projectDto)
    {
        var project = await context.Projects.FindAsync(id);

        if (project == null)
        {
            return NotFound();
        }

        project.Title = projectDto.Title;
        project.Description = projectDto.Description;
        project.ImageUrl = projectDto.ImageUrl;
        project.UpdatedAt = DateTime.Now;

        context.Projects.Update(project);
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var project = await context.Projects.FindAsync(id);

        if (project == null)
        {
            return NotFound();
        }

        context.Projects.Remove(project);
        await context.SaveChangesAsync();

        return NoContent();
    }
}

