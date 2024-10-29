using App.Shared.Dto.Project;
using App.Shared.Models;
using App.Shared.Util.ExceptionHandling;
using Ardalis.Result;
using System.Net.Http.Json;

namespace App.Shared.Services.Project;

public class ProjectService(IHttpClientFactory httpClientFactory) : IProjectService
{
    private readonly HttpClient _dataHttpClient = httpClientFactory.CreateClient("DataApiClient");

    public async Task AddProjectAsync(ProjectAddDto projectAddDto)
    {
        var response = await _dataHttpClient.PostAsJsonAsync("/api/Project", projectAddDto);
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task DeleteProjectAsync(int id)
    {
        var response = await _dataHttpClient.DeleteAsync($"/api/Project/{id}");
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task EditProjectAsync(int id, ProjectEditDto projectEditDto)
    {
        var response = await _dataHttpClient.PutAsJsonAsync($"/api/Project/{id}", projectEditDto);
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task<ProjectViewModel> GetProjectById(int id)
    {
        var response = await _dataHttpClient.GetAsync($"/api/Project/{id}");
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<ProjectViewModel>();
        return result;
    }

    public async Task<List<ProjectViewModel>> GetProjects()
    {
        var response = await _dataHttpClient.GetAsync("/api/Project");
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<List<ProjectViewModel>>();
        return result;
    }

    public async Task MakeActiveProject(int id)
    {
        var response = await _dataHttpClient.PutAsync($"/api/Project/Active/{id}/", null);
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task MakeInActiveProject(int id)
    {
        var response = await _dataHttpClient.PutAsync($"/api/Project/InActive/{id}/", null);
        await response.EnsureSuccessStatusCodeWithApiError();
    }
}