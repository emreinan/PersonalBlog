using App.Client.Models;
using App.Client.Util.ExceptionHandling;

namespace App.Client.Services.Education;

public class EducationService(IHttpClientFactory httpClientFactory) : IEducationService
{
    private readonly HttpClient _dataHttpClient = httpClientFactory.CreateClient("DataApiClient");

    public async Task<List<EducationViewModel>> GetEducations()
    {
        var response = await _dataHttpClient.GetAsync("/api/Education");
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<List<EducationViewModel>>();
        return result;
    }
}
