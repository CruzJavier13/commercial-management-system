using Mod.Inventory.Application.UseCase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Mod.Inventory.Domain.Repositories;
using Mod.Inventory.Infrastructure.Persistence;

namespace Mod.Inventory.Infrastructure
{
    public static class DevendencyInyection
    {
        public static IServiceCollection AddInventoryModule(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'CommerceConnectionString' not found.");

            services.AddScoped<IStockMovementRepository>(provider => new StockMovementRepository(connectionString));

            services.AddScoped<CreateStockMovementUseCase>();
            services.AddScoped<GetStockMovementsByProductUseCase>();
            services.AddScoped<GetAllStockMovementsUseCase>();

            return services;
        }
    }
}
