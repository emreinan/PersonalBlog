using App.Data.Contexts;
using App.Data.Entities.Data;
using App.Shared.Dto.Experience;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExperienceController(DataDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var experiences = await context.Experiences
                .Select(e => new ExperienceDto
                {
                    Title = e.Title,
                    Company = e.Company,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    Description = e.Description
                })
                .ToListAsync();

            return Ok(experiences);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var experience = await context.Experiences.FindAsync(id);
            if (experience == null)
            {
                return NotFound();
            }

            var experienceDto = new ExperienceDto
            {
                Title = experience.Title,
                Company = experience.Company,
                StartDate = experience.StartDate,
                EndDate = experience.EndDate,
                Description = experience.Description
            };

            return Ok(experienceDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExperienceDto experienceDto)
        {
            var experience = new Experience
            {
                Title = experienceDto.Title,
                Company = experienceDto.Company,
                StartDate = experienceDto.StartDate,
                EndDate = experienceDto.EndDate,
                Description = experienceDto.Description,
                CreatedAt = DateTime.UtcNow
            };

            context.Experiences.Add(experience);
            await context.SaveChangesAsync();


            return CreatedAtAction(nameof(GetById), new { id = experience.Id }, experienceDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ExperienceDto experienceDto)
        {
            var experience = await context.Experiences.FindAsync(id);
            if (experience == null)
            {
                return NotFound();
            }

            experience.Title = experienceDto.Title;
            experience.Company = experienceDto.Company;
            experience.StartDate = experienceDto.StartDate;
            experience.EndDate = experienceDto.EndDate;
            experience.Description = experienceDto.Description;
            experience.UpdatedAt = DateTime.UtcNow;

            context.Experiences.Update(experience);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var experience = await context.Experiences.FindAsync(id);
            if (experience == null)
            {
                return NotFound();
            }

            context.Experiences.Remove(experience);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }

}
