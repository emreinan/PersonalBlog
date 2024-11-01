using App.Data.Contexts;
using App.Shared.Dto.AboutMe;
using App.Shared.Services.File;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AboutMeController(DataDbContext context, IFileService fileService,IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAboutMe()
    {
        var aboutMe = await context.AboutMes.SingleOrDefaultAsync();
        if (aboutMe == null)
            return NotFound("AboutMe section not found.");

        var aboutMeDto = mapper.Map<AboutMeResponseDto>(aboutMe);
        return Ok(aboutMeDto);

    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<IActionResult> UpdateAboutMe([FromForm] AboutMeDto aboutMeDto)
    {
        var aboutMe = await context.AboutMes.FindAsync(1);
        if (aboutMe == null)
            return NotFound("AboutMe section not found.");

        aboutMe.Title = aboutMeDto.Title;
        aboutMe.Introduciton = aboutMeDto.Introduciton;

        if (aboutMeDto.Image1 != null)
        {
            var imageUrl1 = await fileService.UploadFileAsync(aboutMeDto.Image1);
            if (string.IsNullOrEmpty(imageUrl1))
                return BadRequest("Image1 upload failed.");

            aboutMe.ImageUrl1 = imageUrl1;
        }

        if (aboutMeDto.Image2 != null)
        {
            var imageUrl2 = await fileService.UploadFileAsync(aboutMeDto.Image2);
            if (string.IsNullOrEmpty(imageUrl2))
                return BadRequest("Image2 upload failed.");

            aboutMe.ImageUrl2 = imageUrl2;
        }

        if (aboutMeDto.Cv != null)
        {
            var cvUrl = await fileService.UploadFileAsync(aboutMeDto.Cv);
            if (string.IsNullOrEmpty(cvUrl))
                return BadRequest("CV upload failed.");

            aboutMe.Cv = cvUrl;
        }

        context.AboutMes.Update(aboutMe);
        await context.SaveChangesAsync();

        return NoContent();
    }

}


