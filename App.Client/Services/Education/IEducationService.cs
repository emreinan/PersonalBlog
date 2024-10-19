using App.Client.Models;

namespace App.Client.Services.Education;

public interface IEducationService
{
    Task<List<EducationViewModel>> GetEducations();
}



