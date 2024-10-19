using App.Data.Contexts;
using App.Data.Entities.Data;
using App.Shared.Dto.Experience;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExperienceController(DataDbContext context,IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var experiences = await context.Experiences.ToListAsync();

            var experienceDtos = mapper.Map<List<ExperienceDto>>(experiences);
            return Ok(experienceDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var experience = await context.Experiences.FindAsync(id);
            if (experience == null)
                return NotFound();

            var experienceDto = mapper.Map<ExperienceDto>(experience);

            return Ok(experienceDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExperienceDto experienceDto)
        {
            var experience =mapper.Map<Experience>(experienceDto);
            experience.CreatedAt = DateTime.UtcNow;

            context.Experiences.Add(experience);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = experience.Id }, experienceDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ExperienceDto experienceDto)
        {
            var experience = await context.Experiences.FindAsync(id);
            if (experience == null)
                return NotFound();

            var experienceUpdate = mapper.Map<Experience>(experienceDto);
            experienceUpdate.UpdatedAt = DateTime.UtcNow;

            context.Experiences.Update(experienceUpdate);
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
