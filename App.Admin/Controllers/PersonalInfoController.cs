using App.Shared.Dto.PersonalInfo;
using App.Shared.Models;
using App.Shared.Services.PersonalInfo;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers;

[Route("PersonalInfo")]
public class PersonalInfoController(IPersonalInfoService personalInfoService, IMapper mapper) : Controller
{
    [HttpGet]
    public async Task<IActionResult> PersonalInfo()
    {
        var personalInfo = await personalInfoService.GetPersonalInfoAsync();

        if (personalInfo == null)
            return NotFound();

        return View(personalInfo);
    }

    [HttpGet("Edit")]
    public async Task<IActionResult> Edit()
    {
        var personalInfo = await personalInfoService.GetPersonalInfoAsync();

        if (personalInfo == null)
            return NotFound();

        var personalInfoViewModel = mapper.Map<PersonalInfoViewModel>(personalInfo);
        return View(personalInfoViewModel);
    }

    [HttpPost("Edit")]
    public async Task<IActionResult> Edit(PersonalInfoViewModel personalInfoViewModel)
    {
        if (!ModelState.IsValid)
            return View(personalInfoViewModel);

        var personalInfoDto = mapper.Map<PersonalInfoDto>(personalInfoViewModel);
        await personalInfoService.UpdatePersonalInfoAsync(personalInfoDto);

        TempData["SuccessMessage"] = "Personal info updated successfully";
        return RedirectToAction(nameof(PersonalInfo));
    }
}
