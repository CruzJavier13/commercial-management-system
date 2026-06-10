using CommercialSystem.Shared.Domain.Repositories;
using Mod.Customers.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Customers.Domain.Repositories
{
    public interface ICustomerRepository : IReadOnlyRepository<Customer>, IWriteOnlyRepository<Customer>
    {
    }
}
