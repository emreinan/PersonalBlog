using App.Client.Models;

namespace App.Client.Services.Project;

public interface IProjectService
{
    Task<List<ProjectViewModel>> GetProjects();
}


