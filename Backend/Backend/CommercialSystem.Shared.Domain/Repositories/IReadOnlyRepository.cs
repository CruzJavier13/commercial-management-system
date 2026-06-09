using System;
using System.Collections.Generic;
using System.Text;

namespace CommercialSystem.Shared.Domain.Repositories
{
    public interface IReadOnlyRepository<T> where T : class 
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
    }
}
