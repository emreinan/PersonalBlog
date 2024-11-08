﻿using App.Data.Contexts;
using App.Shared.Dto.Auth;
using App.Shared.Dto.User;
using App.Shared.Services.File;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace App.Auth.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(AuthDbContext authDbContext,IFileService fileService) : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await authDbContext.Users.FindAsync(id);

        if (user is null)
            return NotFound("User not found.");

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
        var users = await authDbContext.Users.ToListAsync();

        if (users.Count == 0 || users is null)
            return NotFound("Users not found.");

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
        var user = await authDbContext.Users.FindAsync(id);

        if (user is null)
            return NotFound("User not found.");

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
        var user = await authDbContext.Users.FindAsync(id);

        if (user is null)
            return NotFound("User not found.");

        authDbContext.Users.Remove(user);
        await authDbContext.SaveChangesAsync();

        return Ok("User deleted successfully.");
    }

    [Authorize]
    [HttpPost("upload-profile-image")]
    public async Task<IActionResult> UploadProfileImage([FromForm] ProfilePicUpload profilePicUpload)
    {
        var userId = GetUserId();
        if (userId == Guid.Empty)
            return BadRequest("User not found.");

        var user = await authDbContext.Users.FindAsync(userId);

        if (user is null)
            return NotFound("User not found.");

        var result = await fileService.UploadFileAsync(profilePicUpload.File);

        if (!result.IsSuccess)
            return BadRequest("Profile image can not be uploaded.");

        user.ProfilePhotoUrl = result.Value;

        await authDbContext.SaveChangesAsync();
        return Ok(result.Value);
    }

    [Authorize]
    [HttpPut("Activate/{id}")]
    public async Task<IActionResult> ActivateUser(Guid id)
    {
        var user = await authDbContext.Users.FindAsync(id);

        if (user is null)
            return NotFound("User not found.");

        user.IsActive = true;

        authDbContext.Users.Update(user);
        await authDbContext.SaveChangesAsync();

        return Ok("User activated successfully.");
    }

    [Authorize]
    [HttpPut("Deactivate/{id}")]
    public async Task<IActionResult> DeactivateUser(Guid id)
    {
        var user = await authDbContext.Users.FindAsync(id);

        if (user is null)
            return NotFound("User not found.");

        user.IsActive = false;

        authDbContext.Users.Update(user);
        await authDbContext.SaveChangesAsync();

        return Ok("User deactivated successfully.");
    }

    private Guid GetUserId()
    {
        if (!User.Identity.IsAuthenticated)
            return Guid.Empty;
        return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
    }
}
