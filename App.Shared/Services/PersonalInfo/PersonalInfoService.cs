using App.Shared.Dto.PersonalInfo;
using App.Shared.Models;
using App.Shared.Services.Token;
using App.Shared.Util.ExceptionHandling;
using System.Net.Http.Json;

namespace App.Shared.Services.PersonalInfo;

public class PersonalInfoService(IHttpClientFactory httpClientFactory,ITokenService tokenService) : BaseService(httpClientFactory),IPersonalInfoService
{
    public async Task<PersonalInfoViewModel> GetPersonalInfoAsync()
    {
        var response = await _dataHttpClient.GetAsync("/api/PersonalInfo");
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<PersonalInfoViewModel>();
        return result;
    }

    public async Task UpdatePersonalInfoAsync(PersonalInfoDto personalInfoDto)
    {
        DataClientGetToken(tokenService);
        var response = await _dataHttpClient.PutAsJsonAsync("/api/PersonalInfo", personalInfoDto);
        await response.EnsureSuccessStatusCodeWithApiError();
    }
}
