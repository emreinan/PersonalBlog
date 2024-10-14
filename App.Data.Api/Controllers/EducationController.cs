using App.Data.Contexts;
using App.Data.Entities.Data;
using App.Shared.Dto.Education;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EducationController(DataDbContext dbContext) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetAllEducations()
        {
            var educations = await dbContext.Educations.ToListAsync();
            if (educations == null || !educations.Any())
                return NotFound("No educations found.");

            return Ok(educations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEducationById(int id)
        {
            var education = await dbContext.Educations.FindAsync(id);
            if (education == null)
                return NotFound("Education not found.");

            return Ok(education);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEducation([FromBody] EducationDto educationDto)
        {
            if (educationDto == null)
                return BadRequest("Invalid education data.");

            var education = new Education
            {
                School = educationDto.School,
                Degree = educationDto.Degree,
                FieldOfStudy = educationDto.FieldOfStudy,
                StartDate = educationDto.StartDate,
                EndDate = educationDto.EndDate,
                CreatedAt = DateTime.UtcNow
            };

            await dbContext.Educations.AddAsync(education);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEducationById), new { id = education.Id }, educationDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEducation(int id, [FromBody] EducationDto educationDto)
        {

            var education = await dbContext.Educations.FindAsync(id);
            if (education == null)
                return NotFound("Education not found.");

            education.School = educationDto.School;
            education.Degree = educationDto.Degree;
            education.FieldOfStudy = educationDto.FieldOfStudy;
            education.StartDate = educationDto.StartDate;
            education.EndDate = educationDto.EndDate;
            education.UpdatedAt = DateTime.UtcNow;

            dbContext.Educations.Update(education);
            await dbContext.SaveChangesAsync();

            return Ok(educationDto);
        }

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
