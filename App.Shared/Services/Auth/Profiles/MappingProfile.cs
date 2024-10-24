using App.Shared.Dto.Auth;
using App.Shared.Models;
using AutoMapper;

namespace App.Shared.Services.Auth.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<LoginDto, LoginViewModel>().ReverseMap();
        CreateMap<RegisterDto, RegisterViewModel>().ReverseMap();
    }
}
