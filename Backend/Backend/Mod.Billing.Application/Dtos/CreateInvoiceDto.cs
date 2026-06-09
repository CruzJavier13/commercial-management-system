using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Billing.Application.Dtos
{
    public class CreateInvoiceDto
    {
        public string InvoiceNumber { get; set; } = string.Empty;
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal SubTotalAmount { get; set; }
        public decimal TotalBilled { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public List<CreateInvoiceDetailDto> Details { get; set; } = new List<CreateInvoiceDetailDto>();
    }
}
