using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Products.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
