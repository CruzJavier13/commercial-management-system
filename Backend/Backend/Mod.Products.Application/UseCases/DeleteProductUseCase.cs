using Mod.Products.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Products.Application.UseCases
{
    public class DeleteProductUseCase
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductUseCase(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task ExecuteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID del producto proporcionado no es válido.");
            }

            int rowsAffected = await _productRepository.DeleteAsync(id);

            if (rowsAffected == 0)
            {
                throw new KeyNotFoundException($"No se encontró ningún producto activo con el ID {id}.");
            }
        }
    }
}
