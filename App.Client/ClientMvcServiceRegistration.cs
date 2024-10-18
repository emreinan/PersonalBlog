﻿using App.Client.Services.AboutMe;
using App.Client.Services.Auth;
using App.Client.Services.BlogPost;
using App.Client.Services.Comment;
using App.Client.Services.ContactMessage;
using App.Client.Services.Education;
using App.Client.Services.Experience;
using App.Client.Services.PersonalInfo;
using App.Client.Services.Project;
using App.Client.Services.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace App.Client;

public static class ClientMvcServiceRegistration
{
    public static IServiceCollection AddClientMvcServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        GetApiUrl(services, configuration);

        services.AddJwtAuthentication(configuration);
        services.AddScoped<IAboutMeService,AboutMeService>();
        services.AddScoped<IBlogPostService, BlogPostService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IContactMessageService, ContactMessageService>();
        services.AddScoped<IEducationService, EducationService>();
        services.AddScoped<IExperienceService, ExperienceService>();
        services.AddScoped<IPersonalInfoService, PersonalInfoService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IAuthService, HttpAuthService>();
        services.AddScoped<ITokenService, CookieTokenService>();


        return services;
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