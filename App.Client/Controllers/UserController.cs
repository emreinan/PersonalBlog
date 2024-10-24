using App.Shared.Dto.User;
using App.Shared.Models;
using App.Shared.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App.Client.Controllers;

[Authorize]
public class UserController(IUserService userService) : Controller
{

    public async Task<IActionResult> Details()
    {
        var userId = GetUserId();
        var result = await userService.GetUserAsync(userId);

        if (result is null)
            return NotFound("User not found.");
        var dto = result.Value;
        var userViewModel = new UserViewModel
        {
            Id = dto.Id,
            UserName = dto.UserName,
            Email = dto.Email,
            ProfilePhoto = dto.ProfilePhoto,
            CreatedAt = dto.CreatedAt,
            IsActive = dto.IsActive
        };
        return View(userViewModel);

    }

    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        var id = GetUserId();
        var result = await userService.GetUserAsync(id);
        if (result is null)
            return NotFound("User not found.");
        var dto = result.Value;
        var userUpdateViewModel = new UserUpdateViewModel
        {
            UserName = dto.UserName,
            Email = dto.Email
        };
        return View(userUpdateViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UserUpdateViewModel userUpdateViewModel)
    {
        var id = GetUserId();
        if (!ModelState.IsValid)
            return View(userUpdateViewModel);

        var userDto = new UserUpdateDto
        {
            UserName = userUpdateViewModel.UserName,
            Email = userUpdateViewModel.Email
        };
        var result = await userService.UpdateUserAsync(id, userDto);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return RedirectToAction(nameof(Details), new { userId = id });
    }

    [HttpPost]
    public async Task<IActionResult> Delete()
    {
        var userId = GetUserId();
        var result = await userService.DeleteUserAsync(userId);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> UploadProfilePhoto(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError(string.Empty, "Invalid file.");
            return View();
        }
        var result = await userService.UploadProfilePhotoAsync(file);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        await SavePhotoToLocalAsync(file);

        return RedirectToAction(nameof(Details));
    }
    public Guid GetUserId()
    {
        return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
    }
    private async Task SavePhotoToLocalAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File cannot be null or empty", nameof(file));

        var originalFileName = file.FileName; 
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", originalFileName);

        using var fileStream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(fileStream); // Yükleme sırasında dosyanın içeriğini kullan
    }
}
