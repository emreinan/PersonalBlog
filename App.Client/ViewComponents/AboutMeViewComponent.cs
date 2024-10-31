using App.Shared.Models;
using App.Shared.Services.AboutMe;
using App.Shared.Services.PersonalInfo;
using Microsoft.AspNetCore.Mvc;

namespace App.Client.ViewComponents;

public class AboutMeViewComponent(IAboutMeService aboutMeService, IPersonalInfoService personalInfoService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(bool useHeroView)
    {
        var aboutMe = await aboutMeService.GetAboutMeAsync();
        var personalInfo = await personalInfoService.GetPersonalInfoAsync();

        var model = new PersonalDetailViewModel
        {
            AboutMe = aboutMe,
            PersonalInfo = personalInfo
        };
        if (useHeroView)
        {
            return View("Hero", model);
        }

        return View("AboutMe", model);
    }

}
