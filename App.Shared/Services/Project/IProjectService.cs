using App.Shared.Dto.Project;
using App.Shared.Models;

namespace App.Shared.Services.Project;

public interface IProjectService
{
    Task<List<ProjectViewModel>> GetProjects();
    Task<ProjectViewModel> GetProjectById(int id);
    Task<ProjectViewModel> AddProjectAsync(ProjectDto projectDto);
    Task EditProjectAsync(int id, ProjectDto projectDto);
    Task DeleteProjectAsync(int id);
    Task MakeActiveProject(int id);
    Task MakeInActiveProject(int id);

}


