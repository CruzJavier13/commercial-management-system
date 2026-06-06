using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Products.Domain.Entities
{
    public class MedicineAttributes
    {
        public int ProductId { get; set; }
        public string HealthRegisterNumber { get; set; } = string.Empty;
        public string ActiveIngredient { get; set; } = string.Empty;
        public bool ExpirationDateRequired { get; set; } = true;
        public bool RequiresPrescription { get; set; } = false;
        public Product? Product { get; set; }
    }
}
