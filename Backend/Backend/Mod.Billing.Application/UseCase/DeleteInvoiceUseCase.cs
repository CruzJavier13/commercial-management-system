using CommercialSystem.Shared.Domain.Repositories;
using Mod.Billing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Billing.Application.UseCase
{
    public class DeleteInvoiceUseCase
    {
        private readonly IWriteOnlyRepository<Invoice> _invoiceRepository;

        public DeleteInvoiceUseCase(IWriteOnlyRepository<Invoice> invoiceRepository) => _invoiceRepository = invoiceRepository;

        public async Task ExecuteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("El ID de la factura no es válido.");

            await _invoiceRepository.DeleteAsync(id);
        }
    }
}
