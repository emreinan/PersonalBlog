using App.Shared.Dto.Education;
using App.Shared.Models;
using App.Shared.Services.Education;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers;

[Route("Education")]
public class EducationController(IEducationService educationService, IMapper mapper) : Controller
{
    [HttpGet("Educations")]
    public async Task<IActionResult> Educations()
    {
        var educations = await educationService.GetEducationsAsync(); 
        return View(educations);
    }

    [HttpGet("Add")]
    public IActionResult Add()
    {
        return View(); 
    }

    [HttpPost("Add")]
    public async Task<IActionResult> Add(EducationSaveViewModel educationViewModel)
    {
        if (!ModelState.IsValid)
            return View(educationViewModel); 

        var educationDto = mapper.Map<EducationSaveDto>(educationViewModel); 
        await educationService.AddEducationAsync(educationDto); 

        TempData["SuccessMessage"] = "Education added successfully";
        return RedirectToAction(nameof(Educations)); 
    }

    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var education = await educationService.GetEducationByIdAsync(id); 

        if (education == null)
            return NotFound(); 

        var educationViewModel = mapper.Map<EducationSaveViewModel>(education); 
        return View(educationViewModel); 
    }

    [HttpPost("Edit/{id}")]
    public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] EducationSaveViewModel educationViewModel)
    {
        if (!ModelState.IsValid)
            return View(educationViewModel); 

        var educationDto = mapper.Map<EducationSaveDto>(educationViewModel); 
        await educationService.EditEducationAsync(id, educationDto);

        TempData["SuccessMessage"] = "Education updated successfully";
        return RedirectToAction(nameof(Educations)); 
    }

    [HttpGet("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var education = await educationService.GetEducationByIdAsync(id); 

        if (education == null)
            return NotFound(); 

        await educationService.DeleteEducationByIdAsync(id);

        TempData["SuccessMessage"] = "Education deleted successfully";
        return RedirectToAction(nameof(Educations));
    }
}
