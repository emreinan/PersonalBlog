using App.Shared.Dto.PersonalInfo;
using App.Shared.Models;
using App.Shared.Util.ExceptionHandling;
using System.Net.Http.Json;

namespace App.Shared.Services.PersonalInfo;

public class PersonalInfoService(IHttpClientFactory httpClientFactory) : IPersonalInfoService
{
    private readonly HttpClient _dataHttpClient = httpClientFactory.CreateClient("DataApiClient");

    public async Task<PersonalInfoViewModel> GetPersonalInfoAsync()
    {
        var response = await _dataHttpClient.GetAsync("/api/PersonalInfo");
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<PersonalInfoViewModel>();
        return result;
    }

    public async Task UpdatePersonalInfoAsync(PersonalInfoDto personalInfoDto)
    {
        var response = await _dataHttpClient.PutAsJsonAsync("/api/PersonalInfo", personalInfoDto);
        await response.EnsureSuccessStatusCodeWithApiError();
    }
}
