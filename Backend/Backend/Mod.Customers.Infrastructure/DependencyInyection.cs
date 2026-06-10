using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mod.Billing.Application.UseCase;
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
            services.AddScoped<CreateInvoiceUseCase>();
            services.AddScoped<GetInvoiceByIdUseCase>();
            services.AddScoped<GetAllInvoiceUseCase>();
            services.AddScoped<UpdateInvoiceUseCase>();
            services.AddScoped<DeleteInvoiceUseCase>();

            return services;
        }
    }
}
