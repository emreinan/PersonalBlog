using App.Client.Models;
using App.Shared.Dto.Auth;
using AutoMapper;

namespace App.Client.Services.Auth.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<LoginDto, LoginViewModel>().ReverseMap();
        CreateMap<RegisterDto, RegisterViewModel>().ReverseMap();
    }
}
