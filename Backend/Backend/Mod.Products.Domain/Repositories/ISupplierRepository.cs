using CommercialSystem.Shared.Domain.Repositories;
using Mod.Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Products.Domain.Repositories
{
    public interface ISupplierRepository : IWriteOnlyRepository<Supplier>, IReadOnlyRepository<Supplier>
    {
    }
}
