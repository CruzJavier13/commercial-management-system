using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Billing.Infrastructure
{
    public static class DependencyInyection
    {
        public static IServiceCollection AddBillingModule(this IServiceCollection services)
        {
            return services;
        }
    }
}
