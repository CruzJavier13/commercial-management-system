using Mod.Products.Application.DTOs;
using Mod.Products.Domain.Entities;
using Mod.Products.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Products.Application.UseCases
{
    public class UpdateCategoryUseCase
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryUseCase(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public async Task ExecuteAsync(int id, UpdateCategoryDto command)
        {
            if (id <= 0)
                throw new ArgumentException("El ID de la categoría proporcionado no es válido.");

            if (string.IsNullOrWhiteSpace(command.CategoryCode))
                throw new ArgumentException("El código de la categoría es obligatorio.");

            if (string.IsNullOrWhiteSpace(command.Name))
                throw new ArgumentException("El nombre de la categoría es obligatorio.");

            var category = new Category
            {
                Id = id, 
                CategoryCode = command.CategoryCode.Trim().ToUpper(),
                Name = command.Name.Trim(),
                Description = string.IsNullOrWhiteSpace(command.Description) ? null : command.Description.Trim(),
                IsActive = true 
            };

            await _categoryRepository.UpdateAsync(category, id);
        }
    }
}
