using App.Client.Models;
using System.Net.Http;

namespace App.Client.Services.PersonalInfo;

public interface IPersonalInfoService
{
    Task<PersonalInfoViewModel> GetPersonalInfo();
}
