using App.Shared.Models;

namespace App.Shared.Services.AboutMe;

public interface IAboutMeService
{
    Task<AboutMeViewModel> GetAboutMe();
}
