using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Sales.Application.DTOs
{
    public class OrderDetailDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtSale { get; set; }
    }
}
