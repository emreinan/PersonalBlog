using App.Shared.Dto.Auth;
using App.Shared.Dto.User;
using App.Shared.Services.Abstract;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Json;

namespace App.Shared.Services.Concrate;

public class UserService(IHttpClientFactory httpClientFactory) : IUserService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("AuthApiClient");

    public async Task<Result> DeleteUserAsync(Guid userId)
    {
        var response = await _httpClient.DeleteAsync($"/api/User/{userId}");
        response.EnsureSuccessStatusCode();
        return new Result();
    }

    public async Task<Result<UserGetResult>> GetUserAsync(Guid userId)
    {
        var response = await _httpClient.GetAsync($"/api/User/{userId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Result<UserGetResult>>();
    }

    public async Task<Result<List<UserGetResult>>> GetUsersAsync()
    {
        var response = await _httpClient.GetAsync("/api/User");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Result<List<UserGetResult>>>();
    }

    public async Task<Result> UpdateUserAsync(Guid id, UserUpdateDto userUpdateDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/User/{id}", userUpdateDto);
        response.EnsureSuccessStatusCode();
        return new Result();
    }

    public async Task<Result> UploadProfilePhotoAsync(IFormFile file)
    {
        var fileUpload = new ProfilePicUpload { File = file };
        var response = await _httpClient.PostAsJsonAsync($"/api/User/UploadProfilePhoto", fileUpload);
        response.EnsureSuccessStatusCode();
        return new Result();
    }
}
