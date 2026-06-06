using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Sales.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; } = 0.00m;
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    }
}
