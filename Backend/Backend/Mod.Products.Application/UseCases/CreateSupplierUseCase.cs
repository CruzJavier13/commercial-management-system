using Mod.Products.Application.DTOs;
using Mod.Products.Domain.Entities;
using Mod.Products.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Products.Application.UseCases
{
    public class CreateSupplierUseCase
    {
        private readonly ISupplierRepository _supplierRepository;

        public CreateSupplierUseCase(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository ?? throw new ArgumentNullException(nameof(supplierRepository));
        }

        public async Task ExecuteAsync(CreateSupplierDto command)
        {
            if (string.IsNullOrWhiteSpace(command.SupplierCode))
                throw new ArgumentException("El código del proveedor es obligatorio.");

            if (string.IsNullOrWhiteSpace(command.CompanyName))
                throw new ArgumentException("El nombre o razón social de la empresa es obligatorio.");

            if (string.IsNullOrWhiteSpace(command.TaxIdentification))
                throw new ArgumentException("La identificación fiscal (RUC / Cédula) es obligatoria.");

            var supplier = new Supplier
            {
                SupplierCode = command.SupplierCode.Trim().ToUpper(),
                CompanyName = command.CompanyName.Trim(),
                TaxIdentification = command.TaxIdentification.Trim().ToUpper(),
                Email = string.IsNullOrWhiteSpace(command.Email) ? null : command.Email.Trim().ToLower(),
                PhoneNumber = string.IsNullOrWhiteSpace(command.PhoneNumber) ? null : command.PhoneNumber.Trim(),
                Address = string.IsNullOrWhiteSpace(command.Address) ? null : command.Address.Trim(),
                IsActive = true 
            };

            await _supplierRepository.SaveAsync(supplier);
        }
    }
}
