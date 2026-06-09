using Mod.Billing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Billing.Application.Dtos
{
    public class GetInvoiceDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal SubTotalAmount { get; set; }
        public decimal TotalBilled { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public List<GetInvoiceDetailDto> Details { get; set; } = new();
    }
}
