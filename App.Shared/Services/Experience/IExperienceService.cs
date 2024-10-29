using App.Shared.Dto.Experience;
using App.Shared.Models;

namespace App.Shared.Services.Experience;

public interface IExperienceService
{
    Task<List<ExperienceViewModel>> GetExperiences();
    Task<ExperienceViewModel> GetExperienceByIdAsync(int id);
    Task AddExperienceAsync(ExperienceSaveDto experienceDto);
    Task EditExperienceAsync(int id, ExperienceSaveDto experienceDto);
    Task DeleteExperienceAsync(int id);

}

