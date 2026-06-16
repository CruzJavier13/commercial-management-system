using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Products.Application.DTOs
{
    public class MedicineExtensionDto
    {
        public string HealthRegisterNumber { get; set; } = string.Empty;
        public string? ActiveIngredient { get; set; }
        public bool ExpirationDateRequired { get; set; }
        public bool RequiresPrescription { get; set; }
    }
}
