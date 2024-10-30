using App.Shared.Dto.User;
using App.Shared.Models;
using App.Shared.Services.User;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App.Client.Controllers;

[Authorize]
public class UserController(IUserService userService,IMapper mapper) : Controller
{

    public async Task<IActionResult> Details()
    {
        var userId = GetUserId();
        var user = await userService.GetUserAsync(userId);
        if (user is null)
            return NotFound("User not found.");

        return View(user);
    }

    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        var id = GetUserId();
        var user = await userService.GetUserAsync(id);
        if (user is null)
            return NotFound("User not found.");

        var userUpdateViewModel = new UserUpdateViewModel
        {
            UserName = user.UserName,
            Email = user.Email
        };
        return View(userUpdateViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UserUpdateViewModel userUpdateViewModel)
    {
        var id = GetUserId();
        if (!ModelState.IsValid)
            return View(userUpdateViewModel);

        var userDto = mapper.Map<UserUpdateDto>(userUpdateViewModel);

        await userService.UpdateUserAsync(id, userDto);

        return RedirectToAction(nameof(Details), new { userId = id });
    }

    [HttpPost]
    public async Task<IActionResult> Delete()
    {
        var userId = GetUserId();
        await userService.DeleteUserAsync(userId);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> UploadProfilePhoto(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            TempData["ErrorMessage"] = "File cannot be null or empty";
            return View();
        }
        var result = await userService.UploadProfilePhotoAsync(file);

        await SavePhotoToLocalAsync(file);

        return RedirectToAction(nameof(Details));
    }
    public Guid GetUserId()
    {
        if (User.Identity.IsAuthenticated)
            return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        throw new UnauthorizedAccessException("User is not authenticated.");
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
