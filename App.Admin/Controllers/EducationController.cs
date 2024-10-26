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
        var educations = await educationService.GetEducations(); 
        return View(educations);
    }

    [HttpGet("Add")]
    public IActionResult Add()
    {
        return View(); 
    }

    [HttpPost("Add")]
    public async Task<IActionResult> Add(EducationViewModel educationViewModel)
    {
        if (!ModelState.IsValid)
            return View(educationViewModel); 

        var educationDto = mapper.Map<EducationDto>(educationViewModel); 
        await educationService.AddEducationAsync(educationDto); 

        ViewBag.Success = "Education added successfully"; 
        return RedirectToAction(nameof(Educations)); 
    }

    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var education = await educationService.GetEducationByIdAsync(id); 

        if (education == null)
            return NotFound(); 

        var educationViewModel = mapper.Map<EducationViewModel>(education); 
        return View(educationViewModel); 
    }

    [HttpPost("Edit/{id}")]
    public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] EducationViewModel educationViewModel)
    {
        if (!ModelState.IsValid)
            return View(educationViewModel); 

        var educationDto = mapper.Map<EducationDto>(educationViewModel); 
        await educationService.EditEducationAsync(id, educationDto); 

        ViewBag.Success = "Education updated successfully"; 
        return RedirectToAction(nameof(Educations)); 
    }

    [HttpGet("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var education = await educationService.GetEducationByIdAsync(id); 

        if (education == null)
            return NotFound(); 

        await educationService.DeleteEducationByIdAsync(id);

        return RedirectToAction(nameof(Educations));
    }
}
