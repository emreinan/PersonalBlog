using App.Shared.Models;
using System.Net.Http;

namespace App.Shared.Services.Experience;

public interface IExperienceService
{
    Task<List<ExperienceViewModel>> GetExperiences();
}

