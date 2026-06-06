using Mod.Sales.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Sales.Domain.Repositories
{
    public interface ISalesRepository
    {
        Task<int> CreateOrderAsync(Order order, string detailsXml);
    }
}
