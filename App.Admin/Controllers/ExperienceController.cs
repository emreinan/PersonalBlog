using App.Shared.Dto.Experience;
using App.Shared.Models;
using App.Shared.Services.Experience;
using AutoMapper;
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

    [HttpGet("Add")]
    public IActionResult Add()
    {
        return View();
    }

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

    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var experience = await experienceService.GetExperienceByIdAsync(id);

        if (experience == null)
            return NotFound();

        var experienceSaveViewModel = mapper.Map<ExperienceSaveViewModel>(experience);
        return View(experienceSaveViewModel);
    }

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

    [HttpGet("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var experience = await experienceService.GetExperienceByIdAsync(id);

        if (experience == null)
            return NotFound();

        await experienceService.DeleteExperienceAsync(id);

        TempData["SuccessMessage"] = "Experience deleted successfully";
        return RedirectToAction(nameof(Experiences));
    }
}

