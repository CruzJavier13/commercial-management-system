using Mod.Products.Application.DTOs;
using Mod.Products.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Products.Application.UseCases
{
    public class GetByIdSupplierUseCase
    {
        private readonly ISupplierRepository _supplierRepository;

        public GetByIdSupplierUseCase(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository ?? throw new ArgumentNullException(nameof(supplierRepository));
        }

        public async Task<GetSupplierDto?> ExecuteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID del proveedor proporcionado no es válido.");
            }

            var sup = await _supplierRepository.GetByIdAsync(id);

            if (sup == null)
            {
                return null;
            }

            return new GetSupplierDto
            {
                Id = sup.Id,
                SupplierCode = sup.SupplierCode,
                CompanyName = sup.CompanyName,
                TaxIdentification = sup.TaxIdentification,
                Email = sup.Email,
                PhoneNumber = sup.PhoneNumber,
                Address = sup.Address,
                IsActive = sup.IsActive
            };
        }
    }
}
