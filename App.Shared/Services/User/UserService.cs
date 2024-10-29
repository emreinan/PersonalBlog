using App.Shared.Dto.Auth;
using App.Shared.Dto.User;
using App.Shared.Models;
using App.Shared.Util.ExceptionHandling;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Json;

namespace App.Shared.Services.User;

public class UserService(IHttpClientFactory httpClientFactory) : IUserService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("AuthApiClient");

    public async Task ActivateUserAsync(Guid userId)
    {
        var response = await _httpClient.PutAsync($"/api/User/Activate/{userId}", null);
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task DeactivateUserAsync(Guid userId)
    {
        var response = await _httpClient.PutAsync($"/api/User/Deactivate/{userId}", null);
        await response.EnsureSuccessStatusCodeWithApiError();
    }


    public async Task DeleteUserAsync(Guid userId)
    {
        var response = await _httpClient.DeleteAsync($"/api/User/{userId}");
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task<UserViewModel> GetUserAsync(Guid userId)
    {
        var response = await _httpClient.GetAsync($"/api/User/{userId}");
        await response.EnsureSuccessStatusCodeWithApiError();
        return await response.Content.ReadFromJsonAsync<UserViewModel>();
    }

    public async Task<List<UserViewModel>> GetUsersAsync()
    {
        var response = await _httpClient.GetAsync("/api/User");
        await response.EnsureSuccessStatusCodeWithApiError();
        return await response.Content.ReadFromJsonAsync<List<UserViewModel>>();
    }

    public async Task UpdateUserAsync(Guid id, UserUpdateDto userUpdateDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/User/{id}", userUpdateDto);
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task<string> UploadProfilePhotoAsync(IFormFile file)
    {
        var fileUpload = new ProfilePicUpload { File = file };
        var response = await _httpClient.PostAsJsonAsync($"/api/User/upload-profile-image", fileUpload);
        await response.EnsureSuccessStatusCodeWithApiError();
        return await response.Content.ReadAsStringAsync();
    }
}
