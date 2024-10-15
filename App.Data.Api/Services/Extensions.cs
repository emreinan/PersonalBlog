using App.Shared.Services.Abstract;

namespace App.Data.Api.Services;

public static class Extensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddHttpClient("FileApiClient", client =>
        {
            client.BaseAddress = new Uri(configuration["FileApiUrl"]);
        });

        services.AddScoped<IAboutMeService, AboutMeService>();
        return services;
    }
}
