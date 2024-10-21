using App.Data.Contexts;
using App.Shared.Dto.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Auth.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(AuthDbContext authDbContext) : ControllerBase
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
            ProfilePhoto = user.ProfilePhoto
        
        };
        return Ok(userGetResult);
    }

    [HttpGet()]
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
                ProfilePhoto = user.ProfilePhoto
            };
            userGetResults.Add(userGetResult);
        }

        return Ok(userGetResults);
    }

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

}
