using App.Shared.Dto.Experience;
using App.Shared.Models;
using App.Shared.Services.Token;
using App.Shared.Util.ExceptionHandling;
using App.Shared.Util.ExceptionHandling.Types;
using System.Net.Http.Json;

namespace App.Shared.Services.Experience;

public class ExperienceService(IHttpClientFactory httpClientFactory,ITokenService tokenService) : BaseService(httpClientFactory),IExperienceService
{
    public async Task AddExperienceAsync(ExperienceSaveDto experienceDto)
    {
        WebApiClientGetToken(tokenService);
        var response = await _apiHttpClient.PostAsJsonAsync("/api/Experience", experienceDto);
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }

    public async Task DeleteExperienceAsync(int id)
    {
        WebApiClientGetToken(tokenService);
        var response = await _apiHttpClient.DeleteAsync($"/api/Experience/{id}");
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }

    public async Task EditExperienceAsync(int id, ExperienceSaveDto experienceDto)
    {
        WebApiClientGetToken(tokenService);
        var response = await _apiHttpClient.PutAsJsonAsync($"/api/Experience/{id}", experienceDto);
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }

    public async Task<ExperienceViewModel> GetExperienceByIdAsync(int id)
    {
        var response = await _apiHttpClient.GetAsync($"/api/Experience/{id}");
        await response.EnsureSuccessStatusCodeWithProblemDetails();
        var experience = await response.Content.ReadFromJsonAsync<ExperienceViewModel>() ??
            throw new DeserializationException("ExperienceViewModel", response.Content.ReadAsStringAsync().Result);
        return experience;
    }

    public async Task<List<ExperienceViewModel>> GetExperiencesAsync()
    {
        var response = await _apiHttpClient.GetAsync("/api/Experience");
        await response.EnsureSuccessStatusCodeWithProblemDetails();
        var result = await response.Content.ReadFromJsonAsync<List<ExperienceViewModel>>() ??
            throw new DeserializationException("List<ExperienceViewModel>", response.Content.ReadAsStringAsync().Result);
        return result;
    }
}

