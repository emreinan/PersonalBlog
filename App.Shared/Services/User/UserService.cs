using App.Shared.Dto.Auth;
using App.Shared.Dto.User;
using App.Shared.Models;
using App.Shared.Services.Token;
using App.Shared.Util.ExceptionHandling;
using App.Shared.Util.ExceptionHandling.Types;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;

namespace App.Shared.Services.User;

public class UserService(IHttpClientFactory httpClientFactory,ITokenService tokenService) : BaseService(httpClientFactory),IUserService
{
    public async Task ActivateUserAsync(Guid userId)
    {
        WebApiClientGetToken(tokenService);
        var response = await _apiHttpClient.PutAsync($"/api/User/Activate/{userId}", null);
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }

    public async Task DeactivateUserAsync(Guid userId)
    {
        WebApiClientGetToken(tokenService);
        var response = await _apiHttpClient.PutAsync($"/api/User/Deactivate/{userId}", null);
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }


    public async Task DeleteUserAsync(Guid userId)
    {
        WebApiClientGetToken(tokenService);
        var response = await _apiHttpClient.DeleteAsync($"/api/User/{userId}");
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }

    public async Task<UserViewModel> GetUserAsync(Guid userId)
    {
        var response = await _apiHttpClient.GetAsync($"/api/User/{userId}");
        await response.EnsureSuccessStatusCodeWithProblemDetails();
        return await response.Content.ReadFromJsonAsync<UserViewModel>() ??
            throw new DeserializationException("Failed to deserialize user list.");
    }

    public async Task<List<UserViewModel>> GetUsersAsync()
    {
        var response = await _apiHttpClient.GetAsync("/api/User");
        await response.EnsureSuccessStatusCodeWithProblemDetails();
        return await response.Content.ReadFromJsonAsync<List<UserViewModel>>() ??
            throw new DeserializationException("Failed to deserialize user list.");
    }

    public async Task UpdateUserAsync(Guid id, UserUpdateDto userUpdateDto)
    {
        WebApiClientGetToken(tokenService);
        var response = await _apiHttpClient.PutAsJsonAsync($"/api/User/{id}", userUpdateDto);
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }

    public async Task<string> UploadProfilePhotoAsync(IFormFile file)
    {
        WebApiClientGetToken(tokenService);
        var fileUpload = new ProfilePicUpload { File = file };
        var response = await _apiHttpClient.PostAsJsonAsync($"/api/User/upload-profile-image", fileUpload);
        await response.EnsureSuccessStatusCodeWithProblemDetails();
        return await response.Content.ReadAsStringAsync();
    }
}
