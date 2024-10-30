using App.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Client.ViewComponents;

public class ContactMessageViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var model = new ContactMessageViewModel();
        return View(model);
    }
}
