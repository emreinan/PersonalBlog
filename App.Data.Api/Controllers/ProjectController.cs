using App.Data.Contexts;
using App.Data.Entities.Data;
using App.Shared.Dto.Project;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController(DataDbContext context,IMapper mapper) : ControllerBase
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
        var project = await context.Projects.FindAsync(id);

        if (project == null)
            return NotFound();

        var projectDto = mapper.Map<ProjectDto>(project);
        return Ok(projectDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(ProjectDto projectDto)
    {
        var project = mapper.Map<Project>(projectDto);
        project.CreatedAt = DateTime.Now;

        context.Projects.Add(project);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, projectDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(int id, ProjectDto projectDto)
    {
        var project = await context.Projects.FindAsync(id);

        if (project == null)
            return NotFound();

        var projectUpdate = mapper.Map<Project>(projectDto);
        projectUpdate.UpdatedAt = DateTime.Now;

        context.Projects.Update(projectUpdate);
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

