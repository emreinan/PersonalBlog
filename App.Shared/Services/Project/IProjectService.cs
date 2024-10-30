using App.Shared.Dto.Project;
using App.Shared.Models;

namespace App.Shared.Services.Project;

public interface IProjectService
{
    Task<List<ProjectViewModel>> GetProjectsAsync();
    Task<ProjectViewModel> GetProjectByIdAsync(int id);
    Task AddProjectAsync(ProjectAddDto projectAddDto);
    Task EditProjectAsync(int id, ProjectEditDto projectEditDto);
    Task DeleteProjectAsync(int id);
    Task MakeActiveProjectAsync(int id);
    Task MakeInActiveProjectAsync(int id);

}


