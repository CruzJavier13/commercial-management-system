using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Products.Application.DTOs
{
    public class UpdateProductDto
    {
        public string ProductCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsVirtualService { get; set; }

        public string? HealthRegisterNumber { get; set; }
        public string? ActiveIngredient { get; set; }
        public bool ExpirationDateRequired { get; set; }
        public bool RequiresPrescription { get; set; }    

        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? SerialNumberOrIMEI { get; set; } 
        public int WarrantyPeriodMonths { get; set; }
    }
}
