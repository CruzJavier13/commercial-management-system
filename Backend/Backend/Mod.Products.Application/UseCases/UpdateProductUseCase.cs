using Mod.Products.Application.DTOs;
using Mod.Products.Domain.Entities;
using Mod.Products.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Products.Application.UseCases
{
    public class UpdateProductUseCase
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductUseCase(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task ExecuteAsync(int id, UpdateProductDto dto)
        {
            if (id <= 0)
                throw new ArgumentException("El ID del producto proporcionado no es válido.");

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
                Id = id,
                ProductCode = dto.ProductCode.Trim().ToUpper(),
                Name = dto.Name.Trim(),
                Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(),
                CategoryId = dto.CategoryId,
                SupplierId = dto.SupplierId,
                BasePrice = dto.BasePrice,
                IsVirtualService = dto.IsVirtualService,
                IsActive = true, 

                MedicineAttributes = string.IsNullOrWhiteSpace(dto.HealthRegisterNumber) ? null : new MedicineAttributes
                {
                    HealthRegisterNumber = dto.HealthRegisterNumber.Trim().ToUpper(),
                    ActiveIngredient = dto.ActiveIngredient?.Trim(),
                    ExpirationDateRequired = dto.ExpirationDateRequired,
                    RequiresPrescription = dto.RequiresPrescription
                },

                DeviceAttributes = string.IsNullOrWhiteSpace(dto.Brand) ? null : new DeviceAttributes
                {
                    Brand = dto.Brand.Trim(),
                    Model = dto.Model?.Trim(),
                    SerialNumberOrIMEI = dto.SerialNumberOrIMEI?.Trim().ToUpper(),
                    WarrantyPeriodMonths = dto.WarrantyPeriodMonths
                }
            };

            var rowsAffected = await _productRepository.UpdateAsync(product, id);

            if (rowsAffected == null)
            {
                throw new KeyNotFoundException($"No se pudo actualizar el producto. Confirme si el ID {id} es correcto y se encuentra activo.");
            }
        }
    }
}
