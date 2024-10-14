using App.Data.Contexts;
using App.Shared.Dto.AboutMe;
using App.Shared.Services.Abstract;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Api.Services;

public class AboutMeService(DataDbContext dataDbContext,HttpClient httpClient) : IAboutMeService
{
    public async Task<Result> DeleteMeAsync(string imageUrl)
    {
        var aboutMe = await dataDbContext.AboutMes.FirstOrDefaultAsync();
        if (aboutMe == null) 
            return Result.NotFound();

        if (aboutMe.ImageUrl1 == imageUrl)
            aboutMe.ImageUrl1 = null;
        else if (aboutMe.ImageUrl2 == imageUrl)
            aboutMe.ImageUrl2 = null;

        await dataDbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<AboutMeDto>> GetMeAsync()
    {
        var aboutMe = await dataDbContext.AboutMes.FirstOrDefaultAsync();
        if (aboutMe == null)
            return Result<AboutMeDto>.NotFound();

        var aboutMeDto = new AboutMeDto
        {
            Introduciton = aboutMe.Introduciton,
            ImageUrl1 = aboutMe.ImageUrl1,
            ImageUrl2 = aboutMe.ImageUrl2
        };
        return Result<AboutMeDto>.Success(aboutMeDto);
    }

    public async Task<Result> UpdateMeAsync(AboutMeUpdateDto aboutMeUpdateDto)
    {
        var aboutMe = await dataDbContext.AboutMes.FirstOrDefaultAsync();

        if (aboutMe == null)
            return Result.NotFound();

        aboutMe.Introduciton = aboutMeUpdateDto.Introduciton;

        if (aboutMeUpdateDto.ImageUrl1 != null)
        {
            var response = await httpClient.PostAsJsonAsync("https://localhost:7207/api/File/Upload", aboutMeUpdateDto.ImageUrl1);
            if (response.IsSuccessStatusCode)
            {
                var imageUrl = await response.Content.ReadAsStringAsync();
                aboutMe.ImageUrl1 = imageUrl;
            }
        }

        if (aboutMeUpdateDto.ImageUrl2 != null)
        {
            var response = await httpClient.PostAsJsonAsync("https://localhost:7207/api/File/Upload", aboutMeUpdateDto.ImageUrl2);
            if (response.IsSuccessStatusCode)
            {
                var imageUrl = await response.Content.ReadAsStringAsync();
                aboutMe.ImageUrl2 = imageUrl;
            }
        }

        await dataDbContext.SaveChangesAsync();
        return Result.Success();
    }
}
