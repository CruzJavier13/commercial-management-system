using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Billing.Application.Dtos
{
    public class CreateInvoiceDetailDto
    {
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceBilled { get; set; }
        public decimal TaxRate { get; set; } = 15.00m;
        public decimal LineTotal { get; set; }
    }
}
