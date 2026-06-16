using Mod.Products.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Products.Application.UseCases
{
    public class DeleteSupplierUseCase
    {
        private readonly ISupplierRepository _supplierRepository;

        public DeleteSupplierUseCase(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository ?? throw new ArgumentNullException(nameof(supplierRepository));
        }

        public async Task ExecuteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID del proveedor proporcionado no es válido.");
            }

            int rowsAffected = await _supplierRepository.DeleteAsync(id);

            if (rowsAffected == 0)
            {
                throw new KeyNotFoundException($"No se encontró ningún proveedor activo con el ID {id}.");
            }
        }
    }
}
