using Microsoft.Extensions.DependencyInjection;
using Mod.Emp.Domain.Repositories;
using Mod.Emp.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Emp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEmployeesModule(this IServiceCollection services)
        {
            // Registrar Repositorio específico de ADO.NET
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            // Aquí registrarás más adelante tus Casos de Uso/Handlers de MediatR para este módulo
            // services.AddScoped<CreateEmployeeUseCase>(); 

            return services;
        }
    }
}
