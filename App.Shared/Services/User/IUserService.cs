using App.Shared.Dto.User;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;

namespace App.Shared.Services.User;

public interface IUserService
{
    Task<Result<UserGetResult>> GetUserAsync(Guid userId);
    Task<Result<List<UserGetResult>>> GetUsersAsync();
    Task<Result> UpdateUserAsync(Guid id, UserUpdateDto userUpdateDto);
    Task<Result> DeleteUserAsync(Guid userId);
    Task<Result> UploadProfilePhotoAsync(IFormFile file);

}
