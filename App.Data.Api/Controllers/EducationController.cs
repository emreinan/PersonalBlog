﻿using App.Data.Contexts;
using App.Data.Entities.Data;
using App.Shared.Dto.Education;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EducationController(DataDbContext dbContext, IMapper mapper) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetAllEducations()
        {
            var educations = await dbContext.Educations.ToListAsync();

            var educationsDto = mapper.Map<List<EducationDto>>(educations);
            return Ok(educationsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEducationById(int id)
        {
            var education = await dbContext.Educations.FindAsync(id);
            if (education == null)
                return NotFound("Education not found.");

            var educationDto = mapper.Map<EducationDto>(education);
            return Ok(educationDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateEducation([FromBody] EducationSaveDto educationSaveDto)
        {
            if (educationSaveDto == null)
                return BadRequest("Invalid education data.");

            var education = mapper.Map<Education>(educationSaveDto);
            education.CreatedAt = DateTime.UtcNow;

            await dbContext.Educations.AddAsync(education);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEducationById), new { id = education.Id }, educationSaveDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEducation(int id, [FromBody] EducationSaveDto educationSaveDto)
        {
            var education = await dbContext.Educations.FindAsync(id);
            if (education == null)
                return NotFound("Education not found.");

            var educationUpdated = mapper.Map(educationSaveDto, education);
            educationUpdated.UpdatedAt = DateTime.UtcNow;

            dbContext.Educations.Update(educationUpdated);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEducation(int id)
        {
            var education = await dbContext.Educations.FindAsync(id);
            if (education == null)
                return NotFound("Education not found.");

            dbContext.Educations.Remove(education);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
