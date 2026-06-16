using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mod.Products.Application.UseCases;
using Mod.Products.Domain.Repositories;
using Mod.Products.Infrastructure.Persistence;

namespace Mod.Products.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddProductsModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddScoped<ICategoryRepository>(provider => new CategoryRepository(connectionString));
        services.AddScoped<IProductRepository>(provider => new ProductRepository(connectionString));
        services.AddScoped<ISupplierRepository>(provider => new SupplierRepository(connectionString));

        services.AddScoped<CreateCategoryUseCase>();
        services.AddScoped<GetAllCategoryUseCase>();
        services.AddScoped<GetByIdCategoryUseCase>();
        services.AddScoped<UpdateCategoryUseCase>();
        services.AddScoped<DeleteCategoryUseCase>();


        services.AddScoped<CreateProductUseCase>();
        services.AddScoped<GetAllProductUseCase>(); 
        services.AddScoped<GetByIdProductUseCase>();
        services.AddScoped<UpdateProductUseCase>();
        services.AddScoped<DeleteProductUseCase>();

        services.AddScoped<CreateSupplierUseCase>();
        services.AddScoped<GetAllSupplierUseCase>();
        services.AddScoped<GetByIdSupplierUseCase>();
        services.AddScoped<UpdateSupplierUseCase>();
        services.AddScoped<DeleteSupplierUseCase>();

        return services;
    }
}
