using CommercialSystem.Shared.Domain.Repositories;
using Mod.Billing.Application.Dtos;
using Mod.Billing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Billing.Application.UseCase;

public class CreateBillingUseCase
{
    private readonly IWriteOnlyRepository<Invoice> _invoiceWriteRepository;

    public CreateBillingUseCase(IWriteOnlyRepository<Invoice> invoiceWriteRepository)
    {
        _invoiceWriteRepository = invoiceWriteRepository;
    }

    public async Task ExecuteAsync(CreateInvoiceDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.InvoiceNumber))
            throw new ArgumentException("El número de factura no puede estar vacío.");

        if (dto.CustomerId <= 0)
            throw new ArgumentException("El código de cliente debe ser un identificador válido.");

        if (dto.Details == null || !dto.Details.Any())
            throw new ArgumentException("No se puede registrar una factura sin líneas de detalle.");

        var invoice = new Invoice
        {
            InvoiceNumber = dto.InvoiceNumber,
            OrderId = dto.OrderId,
            CustomerId = dto.CustomerId,
            TaxAmount = dto.TaxAmount,
            SubTotalAmount = dto.SubTotalAmount,
            TotalBilled = dto.TotalBilled,
            PaymentMethod = dto.PaymentMethod,
            InvoiceDate = dto.InvoiceDate == default ? DateTime.UtcNow : dto.InvoiceDate
        };

        foreach (var detailDto in dto.Details)
        {
            if (detailDto.ProductId <= 0)
                throw new ArgumentException("El código de producto en el detalle no es válido.");

            if (detailDto.Quantity <= 0)
                throw new ArgumentException($"La cantidad para el producto {detailDto.ProductId} debe ser mayor a cero.");

            if (detailDto.LineTotal <= 0)
                throw new ArgumentException($"El total de línea para el producto {detailDto.ProductId} debe ser mayor a cero.");

            // CORREGIDO: Mapeo limpio respetando la propiedad calculada
            var detail = new InvoiceDetail
            {
                ProductId = detailDto.ProductId,
                Quantity = detailDto.Quantity,
                PriceBilled = detailDto.PriceBilled,
                TaxRate = detailDto.TaxRate
            };

            invoice.Details.Add(detail);
        }

        await _invoiceWriteRepository.SaveAsync(invoice);
    }
}
