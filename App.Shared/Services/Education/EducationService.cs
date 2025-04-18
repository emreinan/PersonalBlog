using App.Shared.Dto.Education;
using App.Shared.Models;
using App.Shared.Services.Token;
using App.Shared.Util.ExceptionHandling;
using App.Shared.Util.ExceptionHandling.Types;
using System.Net.Http.Json;

namespace App.Shared.Services.Education;

public class EducationService(IHttpClientFactory httpClientFactory,ITokenService tokenService) : BaseService(httpClientFactory),IEducationService
{
    public async Task AddEducationAsync(EducationSaveDto educationSaveDto)
    {
        WebApiClientGetToken(tokenService);
        var response = await _apiHttpClient.PostAsJsonAsync("/api/Education", educationSaveDto);
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }

    public async Task DeleteEducationByIdAsync(int id)
    {
        WebApiClientGetToken(tokenService);
        var response = await _apiHttpClient.DeleteAsync($"/api/Education/{id}");
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }

    public async Task EditEducationAsync(int id, EducationSaveDto educationSaveDto)
    {
        WebApiClientGetToken(tokenService);
        var response = await _apiHttpClient.PutAsJsonAsync($"/api/Education/{id}", educationSaveDto);
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }

    public async Task<EducationViewModel> GetEducationByIdAsync(int id)
    {
        var response = await _apiHttpClient.GetAsync($"/api/Education/{id}");
        await response.EnsureSuccessStatusCodeWithProblemDetails();
        var result = await response.Content.ReadFromJsonAsync<EducationViewModel>() ??
            throw new DeserializationException("Failed to deserialize the response content.");
        return result;
    }

    public async Task<List<EducationViewModel>> GetEducationsAsync()
    {
        var response = await _apiHttpClient.GetAsync("/api/Education");
        await response.EnsureSuccessStatusCodeWithProblemDetails();
        var result = await response.Content.ReadFromJsonAsync<List<EducationViewModel>>() ??
            throw new DeserializationException("Failed to deserialize the response content.");
        return result;
    }
}
