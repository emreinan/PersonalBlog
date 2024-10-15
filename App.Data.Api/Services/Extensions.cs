using App.Shared.Dto.AboutMe;
using App.Shared.Services.Abstract;
using App.Shared.Services.Concrate;
using FluentValidation;

namespace App.Data.Api.Services;

public static class Extensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddHttpClient("FileApiClient", client =>
        {
            client.BaseAddress = new Uri(configuration["FileApiUrl"]);
        });

        services.AddScoped<IFileService, FileApiService>();

        return services;
    }
}
