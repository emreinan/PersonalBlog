using App.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data
{
    public static class DataRegistrations
    {
        public static IServiceCollection AddDataDbRegistrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DataDbConnection"));
            });

            return services;
        }

        public static IServiceCollection AddAuthDbRegistrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("AuthDbConnection"));
            });

            return services;
        }
    }
}
