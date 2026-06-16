using Mod.Products.Application.DTOs;
using Mod.Products.Domain.Entities;
using Mod.Products.Domain.Repositories;

namespace Mod.Products.Application.UseCases;

public class CreateProductUseCase
{
    private readonly IProductRepository _productRepository;

    public CreateProductUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task ExecuteAsync(CreateProductDto dto)
    {

        if (string.IsNullOrWhiteSpace(dto.ProductCode))
            throw new ArgumentException("El código del producto es obligatorio.");

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("El nombre del producto es obligatorio.");

        if (dto.CategoryId <= 0)
            throw new ArgumentException("Debe asignar una categoría válida al producto.");

        if (dto.SupplierId <= 0)
            throw new ArgumentException("Debe asignar un proveedor válido al producto.");

        if (dto.BasePrice <= 0)
            throw new ArgumentException("El precio base del producto debe ser mayor a cero.");

        var product = new Product
        {
            ProductCode = dto.ProductCode.Trim().ToUpper(),
            Name = dto.Name.Trim(),
            Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(),
            CategoryId = dto.CategoryId,
            SupplierId = dto.SupplierId,
            BasePrice = dto.BasePrice,
            IsVirtualService = dto.IsVirtualService,
            IsActive = true // Estado inicial activo
        };

        await _productRepository.SaveAsync(product);
    }
}
