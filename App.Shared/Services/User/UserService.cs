using App.Shared.Dto.Auth;
using App.Shared.Dto.User;
using App.Shared.Models;
using App.Shared.Services.Token;
using App.Shared.Util.ExceptionHandling;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Json;

namespace App.Shared.Services.User;

public class UserService(IHttpClientFactory httpClientFactory,ITokenService tokenService) : BaseService(httpClientFactory),IUserService
{
    public async Task ActivateUserAsync(Guid userId)
    {
        AuthClientGetToken(tokenService);
        var response = await _authHttpClient.PutAsync($"/api/User/Activate/{userId}", null);
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task DeactivateUserAsync(Guid userId)
    {
        AuthClientGetToken(tokenService);
        var response = await _authHttpClient.PutAsync($"/api/User/Deactivate/{userId}", null);
        await response.EnsureSuccessStatusCodeWithApiError();
    }


    public async Task DeleteUserAsync(Guid userId)
    {
        AuthClientGetToken(tokenService);
        var response = await _authHttpClient.DeleteAsync($"/api/User/{userId}");
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task<UserViewModel> GetUserAsync(Guid userId)
    {
        var response = await _authHttpClient.GetAsync($"/api/User/{userId}");
        await response.EnsureSuccessStatusCodeWithApiError();
        return await response.Content.ReadFromJsonAsync<UserViewModel>();
    }

    public async Task<List<UserViewModel>> GetUsersAsync()
    {
        var response = await _authHttpClient.GetAsync("/api/User");
        await response.EnsureSuccessStatusCodeWithApiError();
        return await response.Content.ReadFromJsonAsync<List<UserViewModel>>();
    }

    public async Task UpdateUserAsync(Guid id, UserUpdateDto userUpdateDto)
    {
        AuthClientGetToken(tokenService);
        var response = await _authHttpClient.PutAsJsonAsync($"/api/User/{id}", userUpdateDto);
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task<string> UploadProfilePhotoAsync(IFormFile file)
    {
        AuthClientGetToken(tokenService);
        var fileUpload = new ProfilePicUpload { File = file };
        var response = await _authHttpClient.PostAsJsonAsync($"/api/User/upload-profile-image", fileUpload);
        await response.EnsureSuccessStatusCodeWithApiError();
        return await response.Content.ReadAsStringAsync();
    }
}
