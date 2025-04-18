using App.Data.Contexts;
using App.Data.Entities;
using App.Shared.Dto.Experience;
using App.Shared.Util.ExceptionHandling.Types;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExperienceController(AppDbContext context, IMapper mapper) : ControllerBase
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
            var experience = await context.Experiences.FindAsync(id)
                ?? throw new NotFoundException("Experience not found.");

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
            var experience = await context.Experiences.FindAsync(id)
                ?? throw new NotFoundException("Experience not found.");

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
            var experience = await context.Experiences.FindAsync(id)
                ?? throw new NotFoundException("Experience not found.");

            context.Experiences.Remove(experience);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }

}
