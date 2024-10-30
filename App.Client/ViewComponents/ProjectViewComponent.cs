using App.Shared.Services.Project;
using Microsoft.AspNetCore.Mvc;

namespace App.Client.ViewComponents;

public class ProjectViewComponent(IProjectService projectService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var projects = await projectService.GetProjectsAsync();
        return View(projects);
    }
}
