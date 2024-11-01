using App.Shared.Models;
using App.Shared.Services.User;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers;

[Route("User")]
public class UserController(IUserService userService, IMapper mapper) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Users()
    {
        var users = await userService.GetUsersAsync();
        if (users == null)
            return NotFound();

        return View(users);
    }

    [Authorize]
    [HttpGet("Active/{id}")]
    public async Task<IActionResult> Activate([FromRoute] Guid id)
    {
        var user = await userService.GetUserAsync(id);
        if (user == null)
            return NotFound();

        await userService.ActivateUserAsync(id);

        TempData["SuccessMessage"] = "User activated successfully";
        return RedirectToAction(nameof(Users));
    }

    [Authorize]
    [HttpGet("Deactive/{id}")]
    public async Task<IActionResult> Deactivate([FromRoute] Guid id)
    {
        var user = await userService.GetUserAsync(id);
        if (user == null)
            return NotFound();

        await userService.DeactivateUserAsync(id);

        TempData["SuccessMessage"] = "User deactivated successfully";
        return RedirectToAction(nameof(Users));
    }

    [Authorize]
    [HttpGet("Delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var user = await userService.GetUserAsync(id);
        if (user == null)
            return NotFound();

        await userService.DeleteUserAsync(id);

        TempData["SuccessMessage"] = "User deleted successfully";
        return RedirectToAction(nameof(Users));
    }
}
