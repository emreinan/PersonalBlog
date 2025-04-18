using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Shared;

public static class SharedRegistrations
{
    public static IServiceCollection AddSharedServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient("WebApiClient", client =>
        {
            string apiUrl = configuration["App:ApiUrl"] ?? throw new InvalidOperationException("WebApi URL is missing");
            client.BaseAddress = new Uri(apiUrl);
        });
        return services;
    }
}
