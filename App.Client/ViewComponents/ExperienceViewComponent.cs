using App.Shared.Services.Experience;
using Microsoft.AspNetCore.Mvc;

namespace App.Client.ViewComponents;

public class ExperienceViewComponent(IExperienceService experienceService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var experiences = await experienceService.GetExperiencesAsync();
        return View(experiences);
    }
}

