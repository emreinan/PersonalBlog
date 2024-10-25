using App.Shared.Dto.Auth;
using App.Shared.Dto.Project;
using App.Shared.Models;
using AutoMapper;

namespace App.Admin.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<LoginDto, LoginViewModel>().ReverseMap();
        CreateMap<RegisterDto, RegisterViewModel>().ReverseMap();

        CreateMap<ProjectDto, ProjectViewModel>().ReverseMap();
    }
}
