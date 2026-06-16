using Mod.Products.Application.DTOs;
using Mod.Products.Domain.Entities;
using Mod.Products.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Products.Application.UseCases
{
    public class CreateCategoryUseCase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategoryUseCase(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public async Task ExecuteAsync(CreateCategoryDto command)
        {

            if (string.IsNullOrWhiteSpace(command.CategoryCode))
                throw new ArgumentException("El código de la categoría es obligatorio.");

            if (string.IsNullOrWhiteSpace(command.Name))
                throw new ArgumentException("El nombre de la categoría es obligatorio.");

            var category = new Category
            {
                CategoryCode = command.CategoryCode.Trim().ToUpper(),
                Name = command.Name.Trim(),
                Description = string.IsNullOrWhiteSpace(command.Description) ? null : command.Description.Trim(),
                IsActive = true // Estado inicial por defecto
            };


            await _categoryRepository.SaveAsync(category);
        }
    }
}
