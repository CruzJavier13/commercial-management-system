using CommercialSystem.Shared.Domain.Repositories;
using Mod.Billing.Application.Dtos;
using Mod.Billing.Domain.Entities;
using Mod.Billing.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Billing.Application.UseCase
{
    public class UpdateInvoiceUseCase
    {
        private readonly IInvoiceRepository _invoiceWriteRepository;

        public UpdateInvoiceUseCase(IInvoiceRepository invoiceWriteRepository)
        {
            _invoiceWriteRepository = invoiceWriteRepository;
        }

        public async Task ExecuteAsync(int id, UpdateInvoiceDto dto)
        {
            if (id <= 0)
                throw new ArgumentException("El ID de la factura no es válido.");

            if (string.IsNullOrWhiteSpace(dto.InvoiceNumber))
                throw new ArgumentException("El número de factura no puede estar vacío.");

            if (dto.CustomerId <= 0)
                throw new ArgumentException("El código de cliente debe ser válido.");

            if (dto.EmployeeId <= 0)
                throw new ArgumentException("El código de empleado es obligatorio.");

            if (dto.Details == null || !dto.Details.Any())
                throw new ArgumentException("La factura modificada debe contener al menos un detalle.");

            var invoice = new Invoice
            {
                InvoiceNumber = dto.InvoiceNumber,
                CustomerId = dto.CustomerId,
                EmployeeId = dto.EmployeeId,
                PaymentMethod = dto.PaymentMethod,
                InvoiceDate = dto.InvoiceDate == default ? DateTime.UtcNow : dto.InvoiceDate
            };

            foreach (var detailDto in dto.Details)
            {
                if (detailDto.ProductId <= 0)
                    throw new ArgumentException("El código de producto no es válido.");

                if (detailDto.Quantity <= 0)
                    throw new ArgumentException($"La cantidad para el producto ID {detailDto.ProductId} debe ser mayor a cero.");

                if (detailDto.PriceBilled < 0)
                    throw new ArgumentException($"El precio para el producto ID {detailDto.ProductId} no puede ser negativo.");

                var detail = new InvoiceDetail
                {
                    ProductId = detailDto.ProductId,
                    Quantity = detailDto.Quantity,
                    PriceBilled = detailDto.PriceBilled,
                    TaxRate = detailDto.TaxRate
                };

                invoice.Details.Add(detail);
            }

            invoice.SubTotalAmount = invoice.Details.Sum(d => d.LineTotal);
            invoice.TaxAmount = invoice.Details.Sum(d => d.LineTotal * (d.TaxRate / 100));
            invoice.TotalBilled = invoice.SubTotalAmount + invoice.TaxAmount;

            await _invoiceWriteRepository.UpdateAsync(invoice, id);
        }
    }
}
