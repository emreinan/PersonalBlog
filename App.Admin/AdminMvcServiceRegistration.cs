using App.Shared.Services.AboutMe;
using App.Shared.Services.Auth;
using App.Shared.Services.BlogPost;
using App.Shared.Services.Comment;
using App.Shared.Services.ContactMessage;
using App.Shared.Services.Education;
using App.Shared.Services.Experience;
using App.Shared.Services.File;
using App.Shared.Services.Mail;
using App.Shared.Services.PersonalInfo;
using App.Shared.Services.Project;
using App.Shared.Services.Token;
using App.Shared.Services.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace App.Admin;

public static class AdminMvcServiceRegistration
{
    public static IServiceCollection AddAdminMvcServices(this IServiceCollection services, IConfiguration configuration)
    {
        GetApiUrl(services, configuration);
        AddServices(services);
        services.AddHttpContextAccessor();

        services.AddJwtAuthentication(configuration);
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<IAboutMeService, AboutMeService>();
        services.AddScoped<IBlogPostService, BlogPostService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IContactMessageService, ContactMessageService>();
        services.AddScoped<IEducationService, EducationService>();
        services.AddScoped<IExperienceService, ExperienceService>();
        services.AddScoped<IPersonalInfoService, PersonalInfoService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IAuthService, HttpAuthService>();
        services.AddScoped<ITokenService, CookieTokenService>();
        services.AddScoped<IFileService, FileApiService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMailService, SmtpEmailService>();

        services.AddOptions<SmtpConfiguration>().Configure<IConfiguration>((settings, configuration) =>
        {
            configuration.GetSection("SmtpConfiguration").Bind(settings);
        });
    }

    private static void GetApiUrl(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient("FileApiClient", client =>
        {
            string apiUrl = configuration["ExternalApis:FileApiUrl"] ?? throw new InvalidOperationException("FileApi URL is missing");
            client.BaseAddress = new Uri(apiUrl);
        });
        services.AddHttpClient("DataApiClient", client =>
        {
            string apiUrl = configuration["ExternalApis:DataApiUrl"] ?? throw new InvalidOperationException("DataApi URL is missing");
            client.BaseAddress = new Uri(apiUrl);
        });
        services.AddHttpClient("AuthApiClient", client =>
        {
            string apiUrl = configuration["ExternalApis:AuthApiUrl"] ?? throw new InvalidOperationException("AuthApi URL is missing");
            client.BaseAddress = new Uri(apiUrl);
        });
    }

    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        TokenOptions tokenOptions = configuration.GetSection("TokenOptions").Get<TokenOptions>()
                    ?? throw new InvalidOperationException("TokenOptions cant found in configuration");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidIssuer = tokenOptions.Issuer,
                   ValidAudience = tokenOptions.Audience,
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey))
               };

               options.Events = new JwtBearerEvents
               {
                   OnMessageReceived = context => // her requestte token kontrolü yapar.
                   {
                       var tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();
                       context.Token = tokenService.GetAccessToken();

                       return Task.CompletedTask;
                   },

                   OnChallenge = async context => // token süresi dolduğunda refresh token ile yeni token almak için kullanılır.
                   {
                       var tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();
                       var refreshToken = tokenService.GetRefreshToken();

                       if (string.IsNullOrEmpty(refreshToken)) // Refresh token yoksa login sayfasına yönlendir.
                       {
                           context.HandleResponse();
                           context.Response.Redirect("/Login");
                       }

                       try
                       {
                           var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();
                           var tokenResponse = await authService.RefreshTokenAsync(refreshToken);
                           tokenService.SetAccessToken(tokenResponse.AccessToken);
                           tokenService.SetRefreshToken(tokenResponse.RefreshToken);
                           context.HandleResponse();
                           context.Response.Redirect("/"); // main sayfa yani ındex'e gider.
                       }
                       catch
                       {
                           context.HandleResponse();
                           context.Response.Redirect("/Login");
                       }
                   }
               };
           });
        return services;
    }
}
