using CommercialSystem.Shared.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mod.Billing.Application.UseCase;
using Mod.Billing.Domain.Entities;
using Mod.Billing.Domain.Repositories;
using Mod.Billing.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Billing.Infrastructure
{
    public static class DependencyInyection
    {
        public static IServiceCollection AddBillingModule(this IServiceCollection services, IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'CommerceConnectionString' not found.");

            // Repositories 
            services.AddScoped<IInvoiceRepository>(provider => new InvoiceRepository(connectionString));

            //services.AddScoped<IWriteOnlyRepository<Invoice>>(provider => new InvoiceRepository(connectionString));
            //services.AddScoped<IReadOnlyRepository<Invoice>>(provider => new InvoiceRepository(connectionString));

            // Use cases
            services.AddScoped<CreateInvoiceUseCase>();
            services.AddScoped<GetInvoiceByIdUseCase>();
            services.AddScoped<GetAllInvoiceUseCase>();
            services.AddScoped<DeleteInvoiceUseCase>();
            services.AddScoped<UpdateInvoiceUseCase>();

            return services;
        }
    }
}
