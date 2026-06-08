using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Inventory.Infrastructure
{
    public static class DevendencyInyection
    {
        public static IServiceCollection AddInventoryModule(this IServiceCollection services)
        {
            return services;
        }
    }
}
