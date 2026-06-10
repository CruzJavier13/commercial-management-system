using CommercialSystem.Shared.Domain.Repositories;
using Mod.Billing.Application.Dtos;
using Mod.Billing.Domain.Entities;
using Mod.Billing.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Billing.Application.UseCase
{
    public class GetAllInvoiceUseCase
    {
        private readonly IInvoiceRepository _invoiceReadOnlyRepository;

        public GetAllInvoiceUseCase(IInvoiceRepository invoiceReadOnlyRepository)
        {
            _invoiceReadOnlyRepository = invoiceReadOnlyRepository;
        }

        public async Task<IEnumerable<GetInvoiceDto>> ExecuteAsync()
        {

            var invoices = await _invoiceReadOnlyRepository.GetAllAsync();

            return invoices.Select(invoice => new GetInvoiceDto
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                CustomerId = invoice.CustomerId,
                EmployeeId = invoice.EmployeeId,
                TaxAmount = invoice.TaxAmount,
                SubTotalAmount = invoice.SubTotalAmount,
                TotalBilled = invoice.TotalBilled,
                PaymentMethod = invoice.PaymentMethod,
                InvoiceDate = invoice.InvoiceDate,
                IsActive = invoice.IsActive,
                Details = new List<GetInvoiceDetailDto>()
            }).ToList();
        }
    }
}
