using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Products.Application.DTOs
{
    public class DeviceExtensionDto
    {
        public string Brand { get; set; } = string.Empty;
        public string? Model { get; set; }
        public string? SerialNumberOrIMEI { get; set; }
        public int WarrantyPeriodMonths { get; set; }
    }
}
