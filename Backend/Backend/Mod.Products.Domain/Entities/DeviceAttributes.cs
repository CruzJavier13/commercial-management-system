using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Products.Domain.Entities
{
    public class DeviceAttributes
    {
        public int ProductId { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string? SerialNumberOrIMEI { get; set; }
        public int WarrantyPeriodMonths { get; set; } = 0;
        public Product? Product { get; set; }
    }
}
