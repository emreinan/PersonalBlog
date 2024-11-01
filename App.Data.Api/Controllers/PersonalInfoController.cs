using App.Data.Contexts;
using App.Data.Entities.Data;
using App.Shared.Dto.PersonalInfo;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
            var personalInfo = await context.PersonalInfos.SingleOrDefaultAsync();
            var personalInfoDto = mapper.Map<PersonalInfoDto>(personalInfo);
            return Ok(personalInfoDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdatePersonalInfo(PersonalInfoDto personalInfoDto)
        {
            var personalInfo = await context.PersonalInfos.SingleOrDefaultAsync();

            if (personalInfo == null)
                return NotFound();

            var personalInfoUpdated = mapper.Map(personalInfoDto, personalInfo);
            personalInfoUpdated.UpdatedAt = DateTime.Now;

            context.PersonalInfos.Update(personalInfoUpdated);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }

}
