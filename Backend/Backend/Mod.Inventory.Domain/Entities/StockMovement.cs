using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Inventory.Domain.Entities
{
    public class StockMovement
    {
        public long Id { get; set; }
        public int ProductId { get; set; }
        public int? SupplierId { get; set; }
        public string MovementType { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; } = 0.00m;
        public decimal TotalCost { get; set; }
        public string Concept { get; set; } = string.Empty;
        public string? ReferenceDocument { get; set; }
        public DateTime MovementDate { get; set; } = DateTime.UtcNow;
    }
}
