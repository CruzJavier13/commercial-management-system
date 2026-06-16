using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Products.Application.DTOs
{
    public class CreateCategoryDto
    {
        public string CategoryCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
