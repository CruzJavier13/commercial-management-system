using System;
using System.Collections.Generic;
using System.Text;

namespace CommercialSystem.Shared.Domain.Repositories
{
    public interface IWriteOnlyRepository<T> where T : class
    {
        Task SaveAsync(T t);
        Task<int> DeleteAsync(int id);
        Task<T> UpdateAsync(T t, int id);
      
    }
}
