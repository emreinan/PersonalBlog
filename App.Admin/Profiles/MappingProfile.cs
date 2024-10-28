using App.Shared.Dto.Auth;
using App.Shared.Dto.BlogPost;
using App.Shared.Dto.Education;
using App.Shared.Dto.Experience;
using App.Shared.Dto.PersonalInfo;
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

        CreateMap<ExperienceDto, ExperienceViewModel>().ReverseMap();

        CreateMap<EducationDto, EducationViewModel>().ReverseMap();

        CreateMap<PersonalInfoDto, PersonalInfoViewModel>().ReverseMap();

        CreateMap<BlogPostDto, BlogPostResponse>().ReverseMap();

        CreateMap<BlogPostResponse, BlogPostViewModel>().ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));
    }
}
