using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http.Features;
using System.Reflection;


namespace App.File.Api.Services;

public static class Extensions
{
    public static IServiceCollection AddFileService(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation();

        services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 2* 1024 * 1024; // 2MB
            options.ValueLengthLimit = 2 * 1024 * 1024;
            options.MultipartHeadersLengthLimit = 2 * 1024 * 1024;
        });

        return services;
    }
   
}
