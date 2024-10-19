using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;


namespace App.File.Api.Services;

public static class Extensions
{
    public static IServiceCollection AddFileService(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation();

        return services;
    }
}
