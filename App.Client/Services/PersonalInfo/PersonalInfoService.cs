using App.Client.Models;
using App.Client.Util.ExceptionHandling;

namespace App.Client.Services.PersonalInfo;

public class PersonalInfoService(IHttpClientFactory httpClientFactory) : IPersonalInfoService
{
    private readonly HttpClient _dataHttpClient = httpClientFactory.CreateClient("DataApiClient");

    public async Task<PersonalInfoViewModel> GetPersonalInfo()
    {
        var response = await _dataHttpClient.GetAsync("/api/PersonalInfo/GetPersonalInfo");
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<PersonalInfoViewModel>();
        return result;
    }
}
