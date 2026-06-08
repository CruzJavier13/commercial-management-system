using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Sales.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSalesModule(this IServiceCollection services)
        {
            return services;
        }
    }
}
