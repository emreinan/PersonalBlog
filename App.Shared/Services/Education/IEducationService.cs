using App.Shared.Models;

namespace App.Shared.Services.Education;

public interface IEducationService
{
    Task<List<EducationViewModel>> GetEducations();
}



