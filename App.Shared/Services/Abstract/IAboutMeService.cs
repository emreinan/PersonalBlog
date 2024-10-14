using App.Shared.Dto.AboutMe;
using Ardalis.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Shared.Services.Abstract;

public interface IAboutMeService
{
    Task<Result<AboutMeDto>> GetMeAsync();
    Task<Result> UpdateMeAsync(AboutMeUpdateDto aboutMeUpdateDto);
    Task<Result> DeleteMeAsync(string imageUrl);
}
