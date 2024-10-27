using App.Shared.Models;
using App.Shared.Services.User;
using AutoMapper;
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

    [HttpGet("Active/{id}")]
    public async Task<IActionResult> Activate([FromRoute] Guid id)
    {
        var user = await userService.GetUserAsync(id);
        if (user == null)
            return NotFound();

        await userService.ActivateUserAsync(id);
        ViewBag.Success = "User activated successfully";
        return RedirectToAction(nameof(Users));
    }

    [HttpGet("Deactive/{id}")]
    public async Task<IActionResult> Deactivate([FromRoute] Guid id)
    {
        var user = await userService.GetUserAsync(id);
        if (user == null)
            return NotFound();

        await userService.DeactivateUserAsync(id);
        ViewBag.Success = "User deactivated successfully";
        return RedirectToAction(nameof(Users));
    }

    [HttpGet("Delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var user = await userService.GetUserAsync(id);
        if (user == null)
            return NotFound();

        await userService.DeleteUserAsync(id);
        ViewBag.Success = "User deleted successfully";
        return RedirectToAction(nameof(Users));
    }
}
