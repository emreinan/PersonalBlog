
using App.Shared.Services.Abstract;
using App.Shared.Services.Concrate;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;

namespace App.Data.Api.Services;

public static class Extensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddHttpClient("FileApiClient", client =>
        {
            string apiUrl = configuration["FileApiUrl"] ?? throw new InvalidOperationException();
            client.BaseAddress = new Uri(apiUrl);
        });

        services.AddScoped<IFileService, FileApiService>();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation();


        return services;
    }
}
