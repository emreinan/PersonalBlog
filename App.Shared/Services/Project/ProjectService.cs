using App.Shared.Models;
using App.Shared.Util.ExceptionHandling;
using System.Net.Http.Json;

namespace App.Shared.Services.Project;

public class ProjectService(IHttpClientFactory httpClientFactory) : IProjectService
{
    private readonly HttpClient _dataHttpClient = httpClientFactory.CreateClient("DataApiClient");

    public async Task<List<ProjectViewModel>> GetProjects()
    {
        var response = await _dataHttpClient.GetAsync("/api/Project");
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<List<ProjectViewModel>>();
        return result;
    }
}