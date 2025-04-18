using App.Data.Contexts;
using App.Shared.Dto.Auth;
using App.Shared.Dto.User;
using App.Shared.Services.File;
using App.Shared.Util.ExceptionHandling.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace App.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(AppDbContext authDbContext,IFileService fileService) : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await authDbContext.Users.FindAsync(id)
            ?? throw new NotFoundException("User not found.");

        var userGetResult = new UserGetResult
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            IsActive = user.IsActive,
            ProfilePhoto = user.ProfilePhotoUrl,
        };
        return Ok(userGetResult);
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await authDbContext.Users.ToListAsync()
            ?? throw new NotFoundException("Users not found.");

        var userGetResults = new List<UserGetResult>();

        foreach (var user in users)
        {
            var userGetResult = new UserGetResult
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive,
                ProfilePhoto = user.ProfilePhotoUrl
            };
            userGetResults.Add(userGetResult);
        }

        return Ok(userGetResults);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, UserUpdateDto userUpdateDto)
    {
        var user = await authDbContext.Users.FindAsync(id)??
            throw new NotFoundException("User not found.");

        user.UserName = userUpdateDto.UserName;
        user.Email = userUpdateDto.Email;

        authDbContext.Users.Update(user);
        await authDbContext.SaveChangesAsync();

        return Ok("User updated successfully.");
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var user = await authDbContext.Users.FindAsync(id) ??
            throw new NotFoundException("User not found.");

        authDbContext.Users.Remove(user);
        await authDbContext.SaveChangesAsync();

        return Ok("User deleted successfully.");
    }

    [Authorize]
    [HttpPost("upload-profile-image")]
    public async Task<IActionResult> UploadProfileImage([FromForm] ProfilePicUpload profilePicUpload)
    {
        var userId = GetUserId();

        var user = await authDbContext.Users.FindAsync(userId) ??
            throw new NotFoundException("User not found.");

        var result = await fileService.UploadFileAsync(profilePicUpload.File);

        user.ProfilePhotoUrl = result;
        await authDbContext.SaveChangesAsync();
        
        return Ok(result);
    }

    [Authorize]
    [HttpPut("Activate/{id}")]
    public async Task<IActionResult> ActivateUser(Guid id)
    {
        var user = await authDbContext.Users.FindAsync(id) ??
            throw new NotFoundException("User not found.");

        if (user.IsActive)
            throw new BadRequestException("User is already active.");

        user.IsActive = true;

        authDbContext.Users.Update(user);
        await authDbContext.SaveChangesAsync();

        return Ok("User activated successfully.");
    }

    [Authorize]
    [HttpPut("Deactivate/{id}")]
    public async Task<IActionResult> DeactivateUser(Guid id)
    {
        var user = await authDbContext.Users.FindAsync(id) ??
            throw new NotFoundException("User not found.");

        if (!user.IsActive)
            throw new BadRequestException("User is already deactive.");

        user.IsActive = false;

        authDbContext.Users.Update(user);
        await authDbContext.SaveChangesAsync();

        return Ok("User deactivated successfully.");
    }

    private Guid GetUserId()
    {
        if (User?.Identity?.IsAuthenticated != true)
            throw new UnauthorizedException("User is not authenticated.");

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            throw new UnauthorizedException("User ID claim is missing or invalid.");

        return userId;
    }
}
