using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Products.Application.DTOs;

public record CreateProductDto(string Code, string Name, decimal Price, int Stock);
