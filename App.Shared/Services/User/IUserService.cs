using App.Shared.Dto.User;
using App.Shared.Models;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;

namespace App.Shared.Services.User;

public interface IUserService
{
    Task<UserViewModel> GetUserAsync(Guid userId);
    Task<List<UserViewModel>> GetUsersAsync();
    Task UpdateUserAsync(Guid id, UserUpdateDto userUpdateDto);
    Task DeleteUserAsync(Guid userId);
    Task<string> UploadProfilePhotoAsync(IFormFile file);
    Task ActivateUserAsync(Guid userId);
    Task DeactivateUserAsync(Guid userId);

}
