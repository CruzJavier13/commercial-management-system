using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Products.Application.DTOs
{
    public class GetProductDto
    {
        public int Id { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsVirtualService { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public MedicineExtensionDto? MedicineAttributes { get; set; }
        public DeviceExtensionDto? DeviceAttributes { get; set; }
    }
}
