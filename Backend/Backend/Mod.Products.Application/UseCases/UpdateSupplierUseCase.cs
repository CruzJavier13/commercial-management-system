using Mod.Products.Application.DTOs;
using Mod.Products.Domain.Entities;
using Mod.Products.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Products.Application.UseCases
{
    public class UpdateSupplierUseCase
    {
        private readonly ISupplierRepository _supplierRepository;

        public UpdateSupplierUseCase(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository ?? throw new ArgumentNullException(nameof(supplierRepository));
        }

        public async Task ExecuteAsync(int id, CreateSupplierDto command)
        {
            if (id <= 0)
                throw new ArgumentException("El ID del proveedor proporcionado no es válido.");

            if (string.IsNullOrWhiteSpace(command.SupplierCode))
                throw new ArgumentException("El código del proveedor es obligatorio.");

            if (string.IsNullOrWhiteSpace(command.CompanyName))
                throw new ArgumentException("El nombre o razón social de la empresa es obligatorio.");

            if (string.IsNullOrWhiteSpace(command.TaxIdentification))
                throw new ArgumentException("La identificación fiscal (RUC) es obligatoria.");

            var supplier = new Supplier
            {
                Id = id, 
                SupplierCode = command.SupplierCode.Trim().ToUpper(),
                CompanyName = command.CompanyName.Trim(),
                TaxIdentification = command.TaxIdentification.Trim().ToUpper(),
                Email = string.IsNullOrWhiteSpace(command.Email) ? null : command.Email.Trim().ToLower(),
                PhoneNumber = string.IsNullOrWhiteSpace(command.PhoneNumber) ? null : command.PhoneNumber.Trim(),
                Address = string.IsNullOrWhiteSpace(command.Address) ? null : command.Address.Trim(),
                IsActive = true 
            };

            await _supplierRepository.UpdateAsync(supplier, id);
        }
    }
}
