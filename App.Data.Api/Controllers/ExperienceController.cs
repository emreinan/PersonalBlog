using App.Data.Contexts;
using App.Data.Entities.Data;
using App.Shared.Dto.Experience;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExperienceController(DataDbContext context, IMapper mapper) : ControllerBase
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
                return NotFound("Experience not found.");

            var experienceDto = mapper.Map<ExperienceDto>(experience);

            return Ok(experienceDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(ExperienceSaveDto experienceDto)
        {
            var experience = mapper.Map<Experience>(experienceDto);
            experience.CreatedAt = DateTime.UtcNow;

            await context.Experiences.AddAsync(experience);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = experience.Id }, experienceDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ExperienceSaveDto experienceDto)
        {
            var experience = await context.Experiences.FindAsync(id);
            if (experience == null)
                return NotFound("Experience not found.");

            var experienceUpdate = mapper.Map(experienceDto, experience);
            experienceUpdate.UpdatedAt = DateTime.UtcNow;

            context.Experiences.Update(experienceUpdate);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
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
