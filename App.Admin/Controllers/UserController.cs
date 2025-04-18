using App.Shared.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers;

[Route("User")]
public class UserController(IUserService userService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Users()
    {
        var users = await userService.GetUsersAsync();
        return View(users);
    }

    [Authorize]
    [HttpGet("Active/{id}")]
    public async Task<IActionResult> Activate([FromRoute] Guid id)
    {
        await userService.ActivateUserAsync(id);

        TempData["SuccessMessage"] = "User activated successfully";
        return RedirectToAction(nameof(Users));
    }

    [Authorize]
    [HttpGet("Deactive/{id}")]
    public async Task<IActionResult> Deactivate([FromRoute] Guid id)
    {
        await userService.DeactivateUserAsync(id);

        TempData["SuccessMessage"] = "User deactivated successfully";
        return RedirectToAction(nameof(Users));
    }

    [Authorize]
    [HttpGet("Delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await userService.DeleteUserAsync(id);

        TempData["SuccessMessage"] = "User deleted successfully";
        return RedirectToAction(nameof(Users));
    }
}
