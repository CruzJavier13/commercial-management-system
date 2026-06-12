using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mod.Emp.Application.UseCases;
using Mod.Emp.Domain.Repositories;
using Mod.Emp.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Emp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEmployeesModule(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // Registrar Repositorio específico de ADO.NET
            services.AddScoped<IEmployeeRepository>(provider => new EmployeeRepository(connectionString));

            // Aquí registrarás más adelante tus Casos de Uso/Handlers de MediatR para este módulo
            services.AddScoped<CreateEmployeeUseCase>();
            services.AddScoped<GetAllEmployeeUseCase>();
            services.AddScoped<GetByIdEmployeeUseCase>();
            services.AddScoped<UpdateEmployeeUseCase>();
            services.AddScoped<DeleteEmployeeUseCase>();

            return services;
        }
    }
}
