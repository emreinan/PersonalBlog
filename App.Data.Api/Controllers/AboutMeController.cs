using App.Data.Contexts;
using App.Shared.Dto.AboutMe;
using App.Shared.Services.Abstract;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Http;

namespace App.Data.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AboutMeController(DataDbContext context, IFileService fileService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAboutMe()
    {
        var aboutMe = await context.AboutMes.FirstOrDefaultAsync();
        if (aboutMe == null)
            return NotFound("AboutMe section not found.");

        var aboutMeDto = mapper.Map<AboutMeDto>(aboutMe);
        return Ok(aboutMeDto);

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAboutMe(int id, [FromForm] AboutMeDto aboutMeDto)
    {
        var aboutMe = await context.AboutMes.FindAsync(id);
        if (aboutMe == null)
            return NotFound("AboutMe section not found.");

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

        context.AboutMes.Update(aboutMe);
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAboutMe(int id, [FromQuery] string fileName)
    {
        var aboutMe = await context.AboutMes.FindAsync(id);

        if (aboutMe == null)
            return NotFound();

        if (!string.IsNullOrEmpty(fileName))
        {
            var deleteResult = await fileService.DeleteFileAsync(fileName);

            if (!deleteResult.IsSuccess)
                return BadRequest("File deletion failed.");
        }

        if (aboutMe.ImageUrl1 == fileName)
        {
            aboutMe.ImageUrl1 = ""; 
        }
        else if (aboutMe.ImageUrl2 == fileName)
        {
            aboutMe.ImageUrl2 = ""; 
        }

        context.AboutMes.Update(aboutMe);
        await context.SaveChangesAsync();

        return NoContent();
    }
}


