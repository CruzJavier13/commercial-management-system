using CommercialSystem.Shared.Domain.Repositories;
using Mod.Billing.Application.Dtos;
using Mod.Billing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Billing.Application.UseCase
{
    public class GetInvoiceByIdUseCase
    {
        private readonly IReadOnlyRepository<Invoice> _invoiceReadOnlyRepository;

        public GetInvoiceByIdUseCase(IReadOnlyRepository<Invoice> invoiceReadOnlyRepository)
        {
            _invoiceReadOnlyRepository = invoiceReadOnlyRepository;
        }

        public async Task<GetInvoiceDto?> ExecuteAsync(int id)
        {

            if (id <= 0)
                throw new ArgumentException("El ID de la factura proporcionado no es válido.");


            var invoice = await _invoiceReadOnlyRepository.GetByIdAsync(id);

            if (invoice == null)
                return null;

            var invoiceDto = new GetInvoiceDto
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                //OrderId = invoice.OrderId,
                CustomerId = invoice.CustomerId,
                EmployeeId = invoice.EmployeeId,
                TaxAmount = invoice.TaxAmount,
                SubTotalAmount = invoice.SubTotalAmount,
                TotalBilled = invoice.TotalBilled,
                PaymentMethod = invoice.PaymentMethod,
                InvoiceDate = invoice.InvoiceDate
            };

            foreach (var detail in invoice.Details)
            {
                invoiceDto.Details.Add(new GetInvoiceDetailDto
                {
                    Id = detail.Id,
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    PriceBilled = detail.PriceBilled,
                    TaxRate = detail.TaxRate,
                    LineTotal = detail.LineTotal 
                });
            }

            return invoiceDto;
        }

    }
}
