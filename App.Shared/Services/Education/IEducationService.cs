using App.Shared.Dto.Education;
using App.Shared.Models;

namespace App.Shared.Services.Education;

public interface IEducationService
{
    Task<List<EducationViewModel>> GetEducations();
    Task<EducationViewModel> GetEducationByIdAsync(int id);
    Task AddEducationAsync(EducationDto educationDto);
    Task EditEducationAsync(int id, EducationDto educationDto);
    Task DeleteEducationByIdAsync(int id);
}



