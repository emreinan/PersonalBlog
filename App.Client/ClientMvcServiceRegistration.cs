using App.Client.Services.AboutMe;
using App.Client.Services.BlogPost;
using App.Client.Services.Comment;
using App.Client.Services.ContactMessage;
using App.Client.Services.Education;
using App.Client.Services.Experience;
using App.Client.Services.PersonalInfo;
using App.Client.Services.Project;

namespace App.Client;

public static class ClientMvcServiceRegistration
{
    public static IServiceCollection AddClientMvcServices(this IServiceCollection services,IConfiguration configuration)
    {
        GetApiUrl(services, configuration);

        services.AddScoped<IAboutMeService,AboutMeService>();
        services.AddScoped<IBlogPostService, BlogPostService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IContactMessageService, ContactMessageService>();
        services.AddScoped<IEducationService, EducationService>();
        services.AddScoped<IExperienceService, ExperienceService>();
        services.AddScoped<IPersonalInfoService, PersonalInfoService>();
        services.AddScoped<IProjectService, ProjectService>();
        return services;
    }

    private static void GetApiUrl(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient("FileApiClient", client =>
        {
            string apiUrl = configuration["FileApiUrl"] ?? throw new InvalidOperationException();
            client.BaseAddress = new Uri(apiUrl);
        });

        services.AddHttpClient("DataApiClient", client =>
        {
            string apiUrl = configuration["DataApiUrl"] ?? throw new InvalidOperationException();
            client.BaseAddress = new Uri(apiUrl);
        });
        services.AddHttpClient("AuthApiClient", client =>
        {
            string apiUrl = configuration["AuthApiUrl"] ?? throw new InvalidOperationException();
            client.BaseAddress = new Uri(apiUrl);
        });
    }
}
