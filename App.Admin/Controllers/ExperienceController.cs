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
        var experiences = await experienceService.GetExperiences();
        return View(experiences);
    }

    [HttpGet("Add")]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost("Add")]
    public async Task<IActionResult> Add(ExperienceViewModel experienceViewModel)
    {
        if (!ModelState.IsValid)
            return View(experienceViewModel);

        var experienceDto = mapper.Map<ExperienceDto>(experienceViewModel);
        await experienceService.AddExperienceAsync(experienceDto);

        ViewBag.Success = "Experience added successfully";
        return RedirectToAction(nameof(Experiences));
    }

    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var experience = await experienceService.GetExperienceByIdAsync(id);

        if (experience == null)
            return NotFound();

        var experienceViewModel = mapper.Map<ExperienceViewModel>(experience);
        return View(experienceViewModel);
    }

    [HttpPost("Edit/{id}")]
    public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] ExperienceViewModel experienceViewModel)
    {
        if (!ModelState.IsValid)
            return View(experienceViewModel);

        var experienceDto = mapper.Map<ExperienceDto>(experienceViewModel);
        await experienceService.EditExperienceAsync(id, experienceDto);

        ViewBag.Success = "Experience updated successfully";
        return RedirectToAction(nameof(Experiences));
    }

    [HttpGet("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var experience = await experienceService.GetExperienceByIdAsync(id);

        if (experience == null)
            return NotFound();

        await experienceService.DeleteExperienceAsync(id);

        ViewBag.Success = "Experience deleted successfully";
        return RedirectToAction(nameof(Experiences));
    }
}

