using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Inventory.Application.Dtos
{
    public class InventoryStockMovementDto
    {
        public long Id { get; set; }
        public int ProductId { get; set; }
        public int? SupplierId { get; set; }
        public string MovementType { get; set; } = string.Empty; // 'IN' o 'OUT'
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public string Concept { get; set; } = string.Empty;
        public string? ReferenceDocument { get; set; }
        public DateTime MovementDate { get; set; }
    }
}
