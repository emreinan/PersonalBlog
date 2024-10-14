using App.Shared.Services.Abstract;

namespace App.Data.Api.Services;

public static class Extensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddScoped<IAboutMeService, AboutMeService>();
        return services;
    }
}
