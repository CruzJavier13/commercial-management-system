using Microsoft.Extensions.DependencyInjection;
using Mod.Products.Domain.Repositories;
using Mod.Products.Infrastructure.Persistence;
using Mod.Products.Application.UseCases;

namespace Mod.Products.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddProductsModule(this IServiceCollection services)
    {
        

        return services;
    }
}
