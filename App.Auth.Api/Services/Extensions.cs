using App.Auth.Api.Model;
using App.Shared.Dto.Auth;
using App.Shared.Services.Abstract;
using App.Shared.Services.Concrate;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace App.Auth.Api.Services;

public static class Extensions
{
    public static IServiceCollection AddAuthService(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<TokenHelper>();
        services.AddTransient<IMailService, SmtpEmailService>();
        services.AddOptions<SmtpConfiguration>().Configure<IConfiguration>((settings, configuration) =>
        {
            configuration.GetSection("SmtpConfiguration").Bind(settings);
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation();
        JwtAuthentication(services, configuration);
        return services;
    }

    private static void JwtAuthentication(IServiceCollection services, IConfiguration configuration)
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
}
