using App.Data.Contexts;
using App.Shared.Dto.AboutMe;
using App.Shared.Services.File;
using App.Shared.Util.ExceptionHandling.Types;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.WebApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AboutMeController(AppDbContext context, IFileService fileService,IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAboutMe()
    {
        var aboutMe = await context.AboutMes.FirstAsync() 
            ?? throw new NotFoundException("AboutMe section not found.");

        var aboutMeDto = mapper.Map<AboutMeResponseDto>(aboutMe);
        return Ok(aboutMeDto);

    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<IActionResult> UpdateAboutMe([FromForm] AboutMeDto aboutMeDto)
    {
        var aboutMe = await context.AboutMes.SingleOrDefaultAsync() 
            ?? throw new NotFoundException("AboutMe section not found.");

        aboutMe.Title = aboutMeDto.Title;
        aboutMe.Introduciton = aboutMeDto.Introduciton;

        if (aboutMeDto.Image1 is not null)
        {
            var imageUrl1 = await fileService.UploadFileAsync(aboutMeDto.Image1);
            aboutMe.ImageUrl1 = imageUrl1;
        }

        if (aboutMeDto.Image2 is not null)
        {
            var imageUrl2 = await fileService.UploadFileAsync(aboutMeDto.Image2);
            aboutMe.ImageUrl2 = imageUrl2;
        }

        if (aboutMeDto.Cv is not null)
        {
            var cvUrl = await fileService.UploadFileAsync(aboutMeDto.Cv);
            aboutMe.Cv = cvUrl;
        }

        context.AboutMes.Update(aboutMe);
        await context.SaveChangesAsync();

        return NoContent();
    }

}


