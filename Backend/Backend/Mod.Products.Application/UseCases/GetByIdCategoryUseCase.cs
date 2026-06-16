using Mod.Products.Application.DTOs;
using Mod.Products.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Products.Application.UseCases
{
    public class GetByIdCategoryUseCase
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetByIdCategoryUseCase(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public async Task<GetCategoryDto?> ExecuteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID de la categoría proporcionado no es válido.");
            }

            var cat = await _categoryRepository.GetByIdAsync(id);

            if (cat == null)
            {
                return null;
            }

            return new GetCategoryDto
            {
                Id = cat.Id,
                CategoryCode = cat.CategoryCode,
                Name = cat.Name,
                Description = cat.Description,
                IsActive = cat.IsActive,
                CreatedAt = cat.CreatedAt
            };
        }
    }
}
