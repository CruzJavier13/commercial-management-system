using Mod.Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Products.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<int> SaveAsync(Product product);
        Task<Product> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<int> DeleteAsync(int id);
        Task<int> UpdateAsync(Product product);
    }
}
