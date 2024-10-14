using App.Shared.Dto.AboutMe;
using App.Shared.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Data.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AboutMeController(IAboutMeService _aboutMeService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetMeAsync()
    {
        var result = await _aboutMeService.GetMeAsync();
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateMeAsync([FromBody] AboutMeUpdateDto aboutMeUpdateDto)
    {
        var result = await _aboutMeService.UpdateMeAsync(aboutMeUpdateDto);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteMeAsync(string imageUrl)
    {
        var result = await _aboutMeService.DeleteMeAsync(imageUrl);
        return Ok(result);
    }
}
