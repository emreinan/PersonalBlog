using App.Client.Models;
using App.Client.Util.ExceptionHandling;

namespace App.Client.Services.Experience;

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

