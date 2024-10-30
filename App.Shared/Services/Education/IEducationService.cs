using App.Shared.Dto.Education;
using App.Shared.Models;

namespace App.Shared.Services.Education;

public interface IEducationService
{
    Task<List<EducationViewModel>> GetEducationsAsync();
    Task<EducationViewModel> GetEducationByIdAsync(int id);
    Task AddEducationAsync(EducationSaveDto educationSaveDto);
    Task EditEducationAsync(int id, EducationSaveDto educationSaveDto);
    Task DeleteEducationByIdAsync(int id);
}



