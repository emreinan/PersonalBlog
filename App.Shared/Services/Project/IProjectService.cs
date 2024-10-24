using App.Shared.Models;

namespace App.Shared.Services.Project;

public interface IProjectService
{
    Task<List<ProjectViewModel>> GetProjects();
}


