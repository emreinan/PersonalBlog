using App.Shared.Dto.AboutMe;
using App.Shared.Models;

namespace App.Shared.Services.AboutMe;

public interface IAboutMeService
{
    Task<AboutMeViewModel> GetAboutMeAsync();
    Task UpdateAboutMeAsync(AboutMeDto aboutMeDto);
}
