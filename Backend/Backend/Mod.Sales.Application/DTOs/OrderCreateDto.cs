using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Sales.Application.DTOs
{
    public class OrderCreateDto
    {
        public string OrderNumber { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderDetailDto> Details { get; set; } = new();
    }
}
