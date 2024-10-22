using App.Client.Models;
using App.Client.Util.ExceptionHandling;

namespace App.Client.Services.Project;

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