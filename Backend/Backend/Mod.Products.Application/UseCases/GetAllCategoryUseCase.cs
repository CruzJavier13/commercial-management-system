using Mod.Products.Application.DTOs;
using Mod.Products.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Products.Application.UseCases
{
    public class GetAllCategoryUseCase
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetAllCategoryUseCase(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public async Task<IEnumerable<GetCategoryDto>> ExecuteAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            if (categories == null)
            {
                return Enumerable.Empty<GetCategoryDto>();
            }

            return categories.Select(cat => new GetCategoryDto
            {
                Id = cat.Id,
                CategoryCode = cat.CategoryCode,
                Name = cat.Name,
                Description = cat.Description,
                IsActive = cat.IsActive,
                CreatedAt = cat.CreatedAt
            }).ToList();
        }
    }
}
