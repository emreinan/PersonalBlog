using App.Shared.Models;
using App.Shared.Util.ExceptionHandling;
using System.Net.Http.Json;

namespace App.Shared.Services.AboutMe;

public class AboutMeService(IHttpClientFactory httpClientFactory) : IAboutMeService
{
    private readonly HttpClient _dataHttpClient = httpClientFactory.CreateClient("DataApiClient");
    public async Task<AboutMeViewModel> GetAboutMe()
    {
        var response = await _dataHttpClient.GetAsync("/api/AboutMe");
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<AboutMeViewModel>();
        return result;
    }
}
