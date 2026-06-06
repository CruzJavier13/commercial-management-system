using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Billing.Domain.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal SubTotalAmount { get; set; }
        public decimal TotalBilled { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
    }
}
