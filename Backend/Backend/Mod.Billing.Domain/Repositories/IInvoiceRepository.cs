using System;
using System.Collections.Generic;
using System.Text;
using CommercialSystem.Shared.Domain.Repositories;
using Mod.Billing.Domain.Entities;

namespace Mod.Billing.Domain.Repositories
{
    public interface IInvoiceRepository : IReadOnlyRepository<Invoice>, IWriteOnlyRepository<Invoice>
    {
        Task<IEnumerable<Invoice>> GetByDate(DateTime date);
    }
}
