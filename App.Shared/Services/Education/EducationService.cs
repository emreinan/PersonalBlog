using App.Shared.Dto.Education;
using App.Shared.Models;
using App.Shared.Services.Token;
using App.Shared.Util.ExceptionHandling;
using System.Net.Http.Json;

namespace App.Shared.Services.Education;

public class EducationService(IHttpClientFactory httpClientFactory,ITokenService tokenService) : BaseService(httpClientFactory),IEducationService
{
    public async Task AddEducationAsync(EducationSaveDto educationSaveDto)
    {
        DataClientGetToken(tokenService);
        var response = await _dataHttpClient.PostAsJsonAsync("/api/Education", educationSaveDto);
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task DeleteEducationByIdAsync(int id)
    {
        DataClientGetToken(tokenService);
        var response = await _dataHttpClient.DeleteAsync($"/api/Education/{id}");
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task EditEducationAsync(int id, EducationSaveDto educationSaveDto)
    {
        DataClientGetToken(tokenService);
        var response = await _dataHttpClient.PutAsJsonAsync($"/api/Education/{id}", educationSaveDto);
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task<EducationViewModel> GetEducationByIdAsync(int id)
    {
        var response = await _dataHttpClient.GetAsync($"/api/Education/{id}");
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<EducationViewModel>();
        return result;
    }

    public async Task<List<EducationViewModel>> GetEducationsAsync()
    {
        var response = await _dataHttpClient.GetAsync("/api/Education");
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<List<EducationViewModel>>();
        return result;
    }
}
