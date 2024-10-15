using App.Data.Contexts;
using App.Data.Entities.Data;
using App.Shared.Dto.PersonalInfo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonalInfoController(DataDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetPersonalInfo()
        {
            var personalInfo = await context.PersonalInfos.ToListAsync();
            return Ok(personalInfo);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonalInfo(int id)
        {
            var personalInfo = await context.PersonalInfos.FindAsync(id);

            if (personalInfo == null)
            {
                return NotFound();
            }

            return Ok(personalInfo);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePersonalInfo(PersonalInfoDto personalInfoDto)
        {
            var personalInfo = new PersonalInfo
            {
                FirstName = personalInfoDto.FirstName,
                LastName = personalInfoDto.LastName,
                PhoneNumber = personalInfoDto.PhoneNumber,
                Email = personalInfoDto.Email,
                BirthDate = personalInfoDto.BirthDate,
                About = personalInfoDto.About,
                CreatedAt = DateTime.Now
            };

            context.PersonalInfos.Add(personalInfo);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPersonalInfo), new { id = personalInfo.Id }, personalInfo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePersonalInfo(int id, PersonalInfoDto personalInfoDto)
        {
            var personalInfo = await context.PersonalInfos.FindAsync(id);

            if (personalInfo == null)
            {
                return NotFound();
            }

            personalInfo.FirstName = personalInfoDto.FirstName;
            personalInfo.LastName = personalInfoDto.LastName;
            personalInfo.PhoneNumber = personalInfoDto.PhoneNumber;
            personalInfo.Email = personalInfoDto.Email;
            personalInfo.BirthDate = personalInfoDto.BirthDate;
            personalInfo.About = personalInfoDto.About;
            personalInfo.UpdatedAt = DateTime.Now;

            context.PersonalInfos.Update(personalInfo);
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
