using App.Client.Models;
using App.Client.Util.ExceptionHandling;

namespace App.Client.Services.AboutMe;

public class AboutMeService(IHttpClientFactory httpClientFactory) : IAboutMeService
{
    private readonly HttpClient _dataHttpClient = httpClientFactory.CreateClient("DataApiClient");
    public async Task<AboutMeViewModel> GetAboutMe()
    {
        var response = await _dataHttpClient.GetAsync("/api/AboutMe/GetAboutMe");
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<AboutMeViewModel>();
        return result;
    }
}
