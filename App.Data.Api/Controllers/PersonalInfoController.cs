using App.Data.Contexts;
using App.Data.Entities.Data;
using App.Shared.Dto.PersonalInfo;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonalInfoController(DataDbContext context,IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetPersonalInfo()
        {
            var personalInfo = await context.PersonalInfos.FirstOrDefaultAsync();
            var personalInfoDto = mapper.Map<PersonalInfoDto>(personalInfo);
            return Ok(personalInfoDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePersonalInfo(PersonalInfoDto personalInfoDto)
        {
            var personalInfo = mapper.Map<PersonalInfo>(personalInfoDto);
            personalInfo.CreatedAt = DateTime.UtcNow;

            context.PersonalInfos.Add(personalInfo);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPersonalInfo), new { id = personalInfo.Id }, personalInfoDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePersonalInfo(int id, PersonalInfoDto personalInfoDto)
        {
            var personalInfo = await context.PersonalInfos.FindAsync(id);

            if (personalInfo == null)
                return NotFound();

            var personalInfoUpdated = mapper.Map<PersonalInfo>(personalInfoDto);
            personalInfoUpdated.UpdatedAt = DateTime.UtcNow;

            context.PersonalInfos.Update(personalInfoUpdated);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonalInfo(int id)
        {
            var personalInfo = await context.PersonalInfos.FindAsync(id);

            if (personalInfo == null)
            {
                return NotFound();
            }

            context.PersonalInfos.Remove(personalInfo);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }

}
