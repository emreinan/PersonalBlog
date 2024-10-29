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
        CreateMap<AboutMe, AboutMeResponseDto>();

        CreateMap<Project, ProjectDto>();
        CreateMap<ProjectAddDto, Project>();
        CreateMap<ProjectEditDto, Project>().ForMember(dest => dest.IsActive, opt=>opt.MapFrom(src=>true));

        CreateMap<PersonalInfo, PersonalInfoDto>();
        CreateMap<PersonalInfoDto, PersonalInfo>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        CreateMap<Experience, ExperienceDto>();
        CreateMap<ExperienceSaveDto, Experience>();
        CreateMap<ExperienceDto, Experience>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        CreateMap<Education, EducationDto>();
        CreateMap<EducationSaveDto, Education>();
        CreateMap<EducationDto, Education>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        CreateMap<ContactMessage, ContactMessageDto>();
        CreateMap<ContactMessageAddDto, ContactMessage>();

        CreateMap<BlogPost, BlogPostResponse>();
        CreateMap<BlogPostDto, BlogPost>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
    }
}
