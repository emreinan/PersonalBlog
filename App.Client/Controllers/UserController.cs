using App.Client.Models;
using App.Shared.Dto.User;
using App.Shared.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Client.Controllers;

[Authorize]
public class UserController(IUserService userService) : Controller
{

    public async Task<IActionResult> Details(Guid userId)
    {
        var result = await userService.GetUserAsync(userId);

        if (result is null)
            return NotFound("User not found.");
        return View(result);

    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await userService.GetUserAsync(id);
        if (result is null)
            return NotFound("User not found.");
        return View(result);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, UserUpdateViewModel userUpdateViewModel)
    {
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
    public async Task<IActionResult> Delete(Guid userId)
    {
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

        return RedirectToAction("Index", "Home");
    }
}
