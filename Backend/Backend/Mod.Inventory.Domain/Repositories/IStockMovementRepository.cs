using CommercialSystem.Shared.Domain.Repositories;
using Mod.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Inventory.Domain.Repositories
{
    public interface IStockMovementRepository : IReadOnlyRepository<StockMovement>, IWriteOnlyRepository<StockMovement>
    {
        Task<IEnumerable<StockMovement>> GetByProductIdAsync(int productId);
    }
}
