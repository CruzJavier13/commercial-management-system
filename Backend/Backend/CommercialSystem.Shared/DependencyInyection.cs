using CommercialSystem.Shared.Persistence.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommercialSystem.Shared.Persistence
{
    public static class DependencyInyection
    {
        public static IServiceCollection AddSharedPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISqlDbContext, SqlDbContext>();
            return services;
        }
    }
}
