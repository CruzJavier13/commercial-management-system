using System;
using System.Collections.Generic;
using System.Text;

namespace CommercialSystem.Shared.Domain.Repositories
{
    public interface IWriteOnlyRepository<T> where T : class
    {
        Task SaveAsync(T t);
        Task DeleteAsync(Guid id);
        Task<T> UpdateAsync(T t, Guid id);
      
    }
}
