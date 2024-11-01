using App.Shared.Dto.Experience;
using App.Shared.Models;
using App.Shared.Services.Token;
using App.Shared.Util.ExceptionHandling;
using System.Net.Http.Json;

namespace App.Shared.Services.Experience;

public class ExperienceService(IHttpClientFactory httpClientFactory,ITokenService tokenService) : BaseService(httpClientFactory),IExperienceService
{
    public async Task AddExperienceAsync(ExperienceSaveDto experienceDto)
    {
        DataClientGetToken(tokenService);
        var response = await _dataHttpClient.PostAsJsonAsync("/api/Experience", experienceDto);
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task DeleteExperienceAsync(int id)
    {
        DataClientGetToken(tokenService);
        var response = await _dataHttpClient.DeleteAsync($"/api/Experience/{id}");
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task EditExperienceAsync(int id, ExperienceSaveDto experienceDto)
    {
        DataClientGetToken(tokenService);
        var response = await _dataHttpClient.PutAsJsonAsync($"/api/Experience/{id}", experienceDto);
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task<ExperienceViewModel> GetExperienceByIdAsync(int id)
    {
        var response = await _dataHttpClient.GetAsync($"/api/Experience/{id}");
        await response.EnsureSuccessStatusCodeWithApiError();
        var experience = await response.Content.ReadFromJsonAsync<ExperienceViewModel>();
        return experience;
    }

    public async Task<List<ExperienceViewModel>> GetExperiencesAsync()
    {
        var response = await _dataHttpClient.GetAsync("/api/Experience");
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<List<ExperienceViewModel>>();
        return result;
    }
}

