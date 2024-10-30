using App.Shared.Services.Education;
using Microsoft.AspNetCore.Mvc;

namespace App.Client.ViewComponents;

public class EducationViewComponent(IEducationService educationService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var educations = await educationService.GetEducationsAsync();

        return View(educations);
    }
}
