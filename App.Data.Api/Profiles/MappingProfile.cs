using App.Data.Entities.Data;
using App.Shared.Dto.AboutMe;
using App.Shared.Dto.BlogPost;
using App.Shared.Dto.Comment;
using App.Shared.Dto.ContactMessage;
using App.Shared.Dto.Education;
using App.Shared.Dto.Experience;
using App.Shared.Dto.PersonalInfo;
using App.Shared.Dto.Project;
using AutoMapper;

namespace App.Data.Api.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AboutMe, AboutMeDto>();

        CreateMap<Project, ProjectDto>();
        CreateMap<ProjectDto, Project>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        CreateMap<PersonalInfo, PersonalInfoDto>();
        CreateMap<PersonalInfoDto, PersonalInfo>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        CreateMap<Experience, ExperienceDto>();
        CreateMap<ExperienceDto, Experience>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        CreateMap<Education, EducationDto>();
        CreateMap<EducationDto, Education>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        CreateMap<ContactMessage, ContactMessageDto>();
        CreateMap<ContactMessageDto, ContactMessage>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        CreateMap<BlogPost, BlogPostDto>();
        CreateMap<BlogPostDto, BlogPost>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
    }
}
