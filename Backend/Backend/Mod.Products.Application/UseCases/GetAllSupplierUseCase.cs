using Mod.Products.Application.DTOs;
using Mod.Products.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Products.Application.UseCases
{
    public class GetAllSupplierUseCase
    {
        private readonly ISupplierRepository _supplierRepository;

        public GetAllSupplierUseCase(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository ?? throw new ArgumentNullException(nameof(supplierRepository));
        }

        public async Task<IEnumerable<GetSupplierDto>> ExecuteAsync()
        {
            var suppliers = await _supplierRepository.GetAllAsync();

            if (suppliers == null)
            {
                return Enumerable.Empty<GetSupplierDto>();
            }

            return suppliers.Select(sup => new GetSupplierDto
            {
                Id = sup.Id,
                SupplierCode = sup.SupplierCode,
                CompanyName = sup.CompanyName,
                TaxIdentification = sup.TaxIdentification,
                Email = sup.Email,
                PhoneNumber = sup.PhoneNumber,
                Address = sup.Address,
                IsActive = sup.IsActive
            }).ToList();
        }
    }
}
