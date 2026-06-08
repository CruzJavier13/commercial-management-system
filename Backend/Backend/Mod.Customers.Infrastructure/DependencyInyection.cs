using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Customers.Infrastructure
{
    public static class DependencyInyection
    {
        public static IServiceCollection AddCustomersModule(this IServiceCollection services)
        {
            return services;
        }
    }
}
