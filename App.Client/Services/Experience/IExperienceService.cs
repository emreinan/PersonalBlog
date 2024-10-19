using App.Client.Models;
using System.Net.Http;

namespace App.Client.Services.Experience;

public interface IExperienceService
{
    Task<List<ExperienceViewModel>> GetExperiences();
}

