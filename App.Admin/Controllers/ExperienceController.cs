using App.Shared.Dto.Experience;
using App.Shared.Models;
using App.Shared.Services.Experience;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers;

[Route("Experience")]
public class ExperienceController(IExperienceService experienceService, IMapper mapper) : Controller
{
    [HttpGet("Experiences")]
    public async Task<IActionResult> Experiences()
    {
        var experiences = await experienceService.GetExperiencesAsync();
        return View(experiences);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("Add")]
    public IActionResult Add()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("Add")]
    public async Task<IActionResult> Add(ExperienceSaveViewModel experienceViewModel)
    {
        if (!ModelState.IsValid)
            return View(experienceViewModel);

        var experienceDto = mapper.Map<ExperienceSaveDto>(experienceViewModel);
        await experienceService.AddExperienceAsync(experienceDto);

        TempData["SuccessMessage"] = "Experience added successfully";
        return RedirectToAction(nameof(Experiences));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var experience = await experienceService.GetExperienceByIdAsync(id);

        var experienceSaveViewModel = mapper.Map<ExperienceSaveViewModel>(experience);
        return View(experienceSaveViewModel);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("Edit/{id}")]
    public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] ExperienceSaveViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var experienceDto = mapper.Map<ExperienceSaveDto>(model);
        await experienceService.EditExperienceAsync(id, experienceDto);

        TempData["SuccessMessage"] = "Experience updated successfully";
        return RedirectToAction(nameof(Experiences));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await experienceService.DeleteExperienceAsync(id);

        TempData["SuccessMessage"] = "Experience deleted successfully";
        return RedirectToAction(nameof(Experiences));
    }
}

