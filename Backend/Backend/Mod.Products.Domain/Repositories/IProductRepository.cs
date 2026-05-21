using Mod.Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Products.Domain.Repositories
{
    public interface IProductRepository
    {
        void Save(Product product);
        IEnumerable<Product> GetAll();
    }
}
