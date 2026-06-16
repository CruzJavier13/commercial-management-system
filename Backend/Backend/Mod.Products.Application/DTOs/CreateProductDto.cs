using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Products.Application.DTOs;

public class CreateProductDto
{
    public string ProductCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public int SupplierId { get; set; }
    public decimal BasePrice { get; set; }
    public bool IsVirtualService { get; set; }


}
