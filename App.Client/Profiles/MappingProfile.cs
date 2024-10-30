using App.Shared.Dto.Auth;
using App.Shared.Dto.User;
using App.Shared.Models;
using AutoMapper;

namespace App.Client.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<LoginDto, LoginViewModel>().ReverseMap();
        CreateMap<RegisterDto, RegisterViewModel>().ReverseMap();

        CreateMap<UserUpdateViewModel, UserUpdateDto>().ReverseMap();
    }
}
