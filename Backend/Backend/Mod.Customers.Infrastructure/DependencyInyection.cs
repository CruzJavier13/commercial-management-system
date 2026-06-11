using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mod.Customers.Application.UseCases;
using Mod.Customers.Domain.Repositories;
using Mod.Customers.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Customers.Infrastructure
{
    public static class DependencyInyection
    {
        public static IServiceCollection AddCustomersModule(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'ConnectionString' not found.");

            services.AddScoped<ICustomerRepository>(provider => new CustomerRepository(connectionString));

            // Register use cases
            services.AddScoped<CreateCustomerUseCase>();
            services.AddScoped<GetAllCustomerUseCase>();
            services.AddScoped<GetByIdCustomerUseCase>();
            services.AddScoped<UpdateCustomerUseCase>();
            services.AddScoped<DeleteCustomerUseCase>();

            return services;
        }
    }
}
