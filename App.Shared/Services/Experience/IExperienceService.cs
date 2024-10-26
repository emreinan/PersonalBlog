using App.Shared.Dto.Experience;
using App.Shared.Models;

namespace App.Shared.Services.Experience;

public interface IExperienceService
{
    Task<List<ExperienceViewModel>> GetExperiences();
    Task<ExperienceViewModel> GetExperienceByIdAsync(int id);
    Task AddExperienceAsync(ExperienceDto experienceDto);
    Task EditExperienceAsync(int id, ExperienceDto experienceDto);
    Task DeleteExperienceAsync(int id);

}

