using Mod.Customers.Domain.Entities;
using Mod.Customers.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Customers.Infrastructure.Persistence
{
    public class CustomerRepository : ICustomerRepository
    {
        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Customer>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Customer?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(Customer t)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> UpdateAsync(Customer t, int id)
        {
            throw new NotImplementedException();
        }
    }
}
