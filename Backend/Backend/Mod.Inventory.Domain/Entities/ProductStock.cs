using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Inventory.Domain.Entities
{
    public class ProductStock
    {
        public int ProductId { get; set; }
        public int CurrentStock { get; set; } = 0;
        public int MinimumRequired { get; set; } = 5;
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
    }
}
