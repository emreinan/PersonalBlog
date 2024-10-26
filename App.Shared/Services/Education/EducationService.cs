using App.Shared.Dto.Education;
using App.Shared.Models;
using App.Shared.Util.ExceptionHandling;
using System.Net.Http.Json;

namespace App.Shared.Services.Education;

public class EducationService(IHttpClientFactory httpClientFactory) : IEducationService
{
    private readonly HttpClient _dataHttpClient = httpClientFactory.CreateClient("DataApiClient");

    public async Task AddEducationAsync(EducationDto educationDto)
    {
        var response = await _dataHttpClient.PostAsJsonAsync("/api/Education", educationDto);
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task DeleteEducationByIdAsync(int id)
    {
        var response = await _dataHttpClient.DeleteAsync($"/api/Education/{id}");
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task EditEducationAsync(int id, EducationDto educationDto)
    {
        var response = await _dataHttpClient.PutAsJsonAsync($"/api/Education/{id}", educationDto);
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task<EducationViewModel> GetEducationByIdAsync(int id)
    {
        var response = await _dataHttpClient.GetAsync($"/api/Education/{id}");
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<EducationViewModel>();
        return result;
    }

    public async Task<List<EducationViewModel>> GetEducations()
    {
        var response = await _dataHttpClient.GetAsync("/api/Education");
        await response.EnsureSuccessStatusCodeWithApiError();
        var result = await response.Content.ReadFromJsonAsync<List<EducationViewModel>>();
        return result;
    }
}
