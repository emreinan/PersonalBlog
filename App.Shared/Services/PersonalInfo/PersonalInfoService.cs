using App.Shared.Dto.PersonalInfo;
using App.Shared.Models;
using App.Shared.Services.Token;
using App.Shared.Util.ExceptionHandling;
using App.Shared.Util.ExceptionHandling.Types;
using System.Net.Http.Json;

namespace App.Shared.Services.PersonalInfo;

public class PersonalInfoService(IHttpClientFactory httpClientFactory,ITokenService tokenService) : BaseService(httpClientFactory),IPersonalInfoService
{
    public async Task<PersonalInfoViewModel> GetPersonalInfoAsync()
    {
        var response = await _apiHttpClient.GetAsync("/api/PersonalInfo");
        await response.EnsureSuccessStatusCodeWithProblemDetails();
        var result = await response.Content.ReadFromJsonAsync<PersonalInfoViewModel>() ??
            throw new DeserializationException("Failed to deserialize the response content.");
        return result;
    }

    public async Task UpdatePersonalInfoAsync(PersonalInfoDto personalInfoDto)
    {
        WebApiClientGetToken(tokenService);
        var response = await _apiHttpClient.PutAsJsonAsync("/api/PersonalInfo", personalInfoDto);
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }
}
