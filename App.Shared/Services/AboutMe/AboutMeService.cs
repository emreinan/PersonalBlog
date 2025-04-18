using App.Shared.Dto.AboutMe;
using App.Shared.Models;
using App.Shared.Services.Token;
using App.Shared.Util.ExceptionHandling;
using App.Shared.Util.ExceptionHandling.Types;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace App.Shared.Services.AboutMe;

public class AboutMeService(IHttpClientFactory httpClientFactory,ITokenService tokenService) : BaseService(httpClientFactory),IAboutMeService
{
    public async Task<AboutMeViewModel> GetAboutMeAsync()
    {
        var response = await _apiHttpClient.GetAsync("/api/AboutMe");
        await response.EnsureSuccessStatusCodeWithProblemDetails();
        var result = await response.Content.ReadFromJsonAsync<AboutMeViewModel>()
            ?? throw new DeserializationException("Failed to deserialize the response.");
        return result;
    }

    public async Task UpdateAboutMeAsync(AboutMeDto aboutMeDto)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(aboutMeDto.Title), "Title" },
            { new StringContent(aboutMeDto.Introduciton), "Introduciton" }
        };

        if (aboutMeDto.Cv != null)
        {
            var cvContent = new StreamContent(aboutMeDto.Cv.OpenReadStream());
            cvContent.Headers.ContentType = new MediaTypeHeaderValue(aboutMeDto.Cv.ContentType);
            content.Add(cvContent, "Cv", aboutMeDto.Cv.FileName);
        }

        if (aboutMeDto.Image1 != null)
        {
            var image1Content = new StreamContent(aboutMeDto.Image1.OpenReadStream());
            image1Content.Headers.ContentType = new MediaTypeHeaderValue(aboutMeDto.Image1.ContentType);
            content.Add(image1Content, "Image1", aboutMeDto.Image1.FileName);
        }

        if (aboutMeDto.Image2 != null)
        {
            var image2Content = new StreamContent(aboutMeDto.Image2.OpenReadStream());
            image2Content.Headers.ContentType = new MediaTypeHeaderValue(aboutMeDto.Image2.ContentType);
            content.Add(image2Content, "Image2", aboutMeDto.Image2.FileName);
        }

        WebApiClientGetToken(tokenService);
        var response = await _apiHttpClient.PutAsync("/api/AboutMe", content);
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }
  }
