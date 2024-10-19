using App.Data.Contexts;
using App.Shared.Dto.AboutMe;
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
        var aboutMe = await context.AboutMe.FirstOrDefaultAsync();
        if (aboutMe == null)
            return NotFound("AboutMe section not found.");

        var aboutMeDto = mapper.Map<AboutMeDto>(aboutMe);
        return Ok(aboutMeDto);

    }

    // Update AboutMe
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAboutMe(int id, [FromForm] AboutMeDto aboutMeDto)
    {
        var aboutMe = await context.AboutMe.FindAsync(id);
        if (aboutMe == null)
            return NotFound("AboutMe section not found.");

        aboutMe.Introduciton = aboutMeDto.Introduciton;

        if (aboutMeDto.Image1 != null)
        {
            var imageUrl1 = await UploadImageAsync(aboutMeDto.Image1);
            if (string.IsNullOrEmpty(imageUrl1))
                return BadRequest("Image1 upload failed.");

            aboutMe.ImageUrl1 = imageUrl1;
        }

        if (aboutMeDto.Image2 != null)
        {
            var imageUrl2 = await UploadImageAsync(aboutMeDto.Image2);
            if (string.IsNullOrEmpty(imageUrl2))
                return BadRequest("Image2 upload failed.");

            aboutMe.ImageUrl2 = imageUrl2;
        }

        context.AboutMe.Update(aboutMe);
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAboutMe(int id, [FromQuery] string imageUrl)
    {
        var aboutMe = await context.AboutMe.FindAsync(id);

        if (aboutMe == null)
            return NotFound();

        if (!string.IsNullOrEmpty(imageUrl))
        {
            var deleteResult = await DeleteImageAsync(imageUrl);

            if (!deleteResult.IsSuccess)
                return BadRequest("File deletion failed.");
        }

        context.AboutMe.Remove(aboutMe);
        await context.SaveChangesAsync();

        return NoContent();
    }


    private async Task<Result> DeleteImageAsync(string imageUrl)
    {
        var client = httpClientFactory.CreateClient("FileApiClient");
        
        var response = await client.DeleteAsync($"api/File?fileUrl={imageUrl}");

        if (!response.IsSuccessStatusCode)
            return Result.Error("Unexpected error occurred while deleting the file.");

        return Result.Success();

    }
    private async Task<Result<string>> UploadImageAsync(IFormFile image)
    {
        var client = httpClientFactory.CreateClient("FileApiClient");
        var formData = new MultipartFormDataContent();

        using var fileStream = image.OpenReadStream();
        var fileContent = new StreamContent(fileStream);
        formData.Add(fileContent, "file", image.FileName);

        var response = await client.PostAsync("/api/File/Upload", formData);

        if (!response.IsSuccessStatusCode)
            return Result<string>.Error("Unexpected error occurred while uploading the file.");

        var fileUrl = await response.Content.ReadAsStringAsync();
        return Result<string>.Success(fileUrl);

    }
}


