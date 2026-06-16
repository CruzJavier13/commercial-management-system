using Mod.Products.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Products.Application.UseCases
{
    public class DeleteCategoryUseCase
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeleteCategoryUseCase(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public async Task ExecuteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID de la categoría proporcionado no es válido.");
            }

            int rowsAffected = await _categoryRepository.DeleteAsync(id);

            if (rowsAffected == 0)
            {
                throw new KeyNotFoundException($"No se encontró ninguna categoría activa con el ID {id}.");
            }
        }
    }
}
