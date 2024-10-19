using App.Client.Models;

namespace App.Client.Services.AboutMe;

public interface IAboutMeService
{
    Task<AboutMeViewModel> GetAboutMe();
}
