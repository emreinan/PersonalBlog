using App.Shared.Dto.User;
using App.Shared.Models;
using App.Shared.Services.User;
using App.Shared.Util.ExceptionHandling.Types;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App.Client.Controllers;

[Authorize]
public class UserController(IUserService userService,IMapper mapper) : Controller
{
    [HttpGet]
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

        TempData["SuccessMessage"] = "User updated successfully";
        return RedirectToAction(nameof(Details), new { userId = id });
    }

    [HttpPost]
    public async Task<IActionResult> Delete()
    {
        var userId = GetUserId();
        await userService.DeleteUserAsync(userId);

        TempData["SuccessMessage"] = "User deleted successfully";
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


        TempData["SuccessMessage"] = "Profile photo uploaded successfully";
        return RedirectToAction(nameof(Details));
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
