using App.Shared.Models;
using App.Shared.Util.ExceptionHandling;
using System.Net.Http.Json;

namespace App.Shared.Services.Experience;

public class ExperienceService(IHttpClientFactory httpClientFactory) : IExperienceService
{
    private readonly HttpClient _dataHttpClient = httpClientFactory.CreateClient("DataApiClient");

    public async Task<List<ExperienceViewModel>> GetExperiences()
    {
        var response = await _dataHttpClient.GetAsync("/api/Experience");
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<List<ExperienceViewModel>>();
        return result;
    }
}

