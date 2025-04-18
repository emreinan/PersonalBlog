using App.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Data
{
    public static class DataRegistrations
    {
        public static IServiceCollection AddDbRegistrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("SqLite"));
            });

            return services;
        }
    }
}
