using App.Data.Api.Model;
using App.Shared.Services.File;
using App.Shared.Services.Mail;
using App.Shared.Services.PersonalInfo;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace App.Data.Api.Services;

public static class Extensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        AddJwtAuthentication(services,configuration);
        AddSwaggerGen(services);
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddHttpContextAccessor();

        services.AddHttpClient("FileApiClient", client =>
        {
            string apiUrl = configuration["FileApiUrl"] ?? throw new InvalidOperationException();
            client.BaseAddress = new Uri(apiUrl);
        });

        services.AddOptions<SmtpConfiguration>().Configure<IConfiguration>((settings, configuration) =>
        {
            configuration.GetSection("SmtpConfiguration").Bind(settings);
        }); 

        services.AddScoped<IFileService, FileApiService>();
        services.AddScoped<IMailService, SmtpEmailService>();
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(Assembly.Load("App.Shared"));


        return services;
    }
    private static void AddJwtAuthentication(IServiceCollection services, IConfiguration configuration)
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

           });
    }
    private static void AddSwaggerGen(IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition(
                name: "Bearer",
                securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description =
                        "JWT Authorization header using the Bearer scheme. " +
                        "\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below." +
                        "\r\n\r\nExample: \"Bearer 12345.54321\""
                }
            );
            c.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                new string[] { }
            }
                }
            );
        });
    }
}
