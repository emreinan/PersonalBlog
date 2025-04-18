using App.Shared.Dto.Education;
using App.Shared.Models;
using App.Shared.Services.Education;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize(Roles = "Admin")]
    [HttpGet("Add")]
    public IActionResult Add()
    {
        return View(); 
    }

    [Authorize(Roles = "Admin")]
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

    [Authorize(Roles = "Admin")]
    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var education = await educationService.GetEducationByIdAsync(id); 

        var educationViewModel = mapper.Map<EducationSaveViewModel>(education); 
        return View(educationViewModel); 
    }

    [Authorize(Roles = "Admin")]
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

    [Authorize(Roles = "Admin")]
    [HttpGet("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await educationService.DeleteEducationByIdAsync(id);

        TempData["SuccessMessage"] = "Education deleted successfully";
        return RedirectToAction(nameof(Educations));
    }
}
