using App.Data.Contexts;
using App.Shared.Dto.PersonalInfo;
using App.Shared.Util.ExceptionHandling.Types;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonalInfoController(AppDbContext context,IMapper mapper) : ControllerBase
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
            var personalInfo = await context.PersonalInfos.SingleOrDefaultAsync() 
                ?? throw new NotFoundException("PersonalInfo not found");
            
            var personalInfoUpdated = mapper.Map(personalInfoDto, personalInfo);
            personalInfoUpdated.UpdatedAt = DateTime.Now;

            context.PersonalInfos.Update(personalInfoUpdated);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }

}
