namespace App.Client;

public static class ClientMvcServiceRegistration
{
    public static IServiceCollection AddClientMvcServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddHttpClient("FileApiClient", client =>
        {
            string apiUrl = configuration["FileApiUrl"] ?? throw new InvalidOperationException();
            client.BaseAddress = new Uri(apiUrl);
        });

        services.AddHttpClient("DataApiClient", client =>
        {
            string apiUrl = configuration["DataApiUrl"] ?? throw new InvalidOperationException();
            client.BaseAddress = new Uri(apiUrl);
        });
        services.AddHttpClient("AuthApiClient", client =>
        {
            string apiUrl = configuration["AuthApiUrl"] ?? throw new InvalidOperationException();
            client.BaseAddress = new Uri(apiUrl);
        });
        return services;
    }
}
