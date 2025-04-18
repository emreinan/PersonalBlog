using App.Shared.Dto.PersonalInfo;
using App.Shared.Models;

namespace App.Shared.Services.PersonalInfo;

public interface IPersonalInfoService
{
    Task<PersonalInfoViewModel> GetPersonalInfoAsync();
    Task UpdatePersonalInfoAsync(PersonalInfoDto personalInfoDto);
}
