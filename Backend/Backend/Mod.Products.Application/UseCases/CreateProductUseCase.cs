using Mod.Products.Application.DTOs;
using Mod.Products.Domain.Entities;
using Mod.Products.Domain.Repositories;

namespace Mod.Products.Application.UseCases;

public class CreateProductUseCase
{
    private readonly IProductRepository _productRepository;

    public CreateProductUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public void Execute(CreateProductDto dto)
    {
        if (dto.Price <= 0)
            throw new ArgumentException("Product price must be greater than zero.");

    //    var product = new Product
    //    {
    //        Code = dto.Code,
    //        Name = dto.Name,
    //        Price = dto.Price,
    //        Stock = dto.Stock
    //    };

    //    _productRepository.Save(product);
    }
}
