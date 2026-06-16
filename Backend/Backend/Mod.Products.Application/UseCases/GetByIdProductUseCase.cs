using Mod.Products.Application.DTOs;
using Mod.Products.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Products.Application.UseCases
{
    public class GetByIdProductUseCase
    {
        private readonly IProductRepository _productRepository;

        public GetByIdProductUseCase(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<GetProductDto?> ExecuteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID del producto proporcionado no es válido.");
            }

            var p = await _productRepository.GetByIdAsync(id);

            if (p == null)
            {
                return null;
            }

            return new GetProductDto
            {
                Id = p.Id,
                ProductCode = p.ProductCode,
                Name = p.Name,
                Description = p.Description,
                CategoryId = p.CategoryId,
                SupplierId = p.SupplierId,
                BasePrice = p.BasePrice,
                IsVirtualService = p.IsVirtualService,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt,

                MedicineAttributes = p.MedicineAttributes == null ? null : new MedicineExtensionDto
                {
                    HealthRegisterNumber = p.MedicineAttributes.HealthRegisterNumber,
                    ActiveIngredient = p.MedicineAttributes.ActiveIngredient,
                    ExpirationDateRequired = p.MedicineAttributes.ExpirationDateRequired,
                    RequiresPrescription = p.MedicineAttributes.RequiresPrescription
                },

                DeviceAttributes = p.DeviceAttributes == null ? null : new DeviceExtensionDto
                {
                    Brand = p.DeviceAttributes.Brand,
                    Model = p.DeviceAttributes.Model,
                    SerialNumberOrIMEI = p.DeviceAttributes.SerialNumberOrIMEI,
                    WarrantyPeriodMonths = p.DeviceAttributes.WarrantyPeriodMonths
                }
            };
        }
    }
}
