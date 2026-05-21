using System.Data;
using Microsoft.Data.SqlClient;
using CommercialSystem.Shared.Persistence.Database; // Matches your exact Shared project namespace
using Mod.Products.Domain.Entities;
using Mod.Products.Domain.Repositories;

namespace Mod.Products.Infrastructure.Persistence;

public class ProductRepository : IProductRepository
{
    private readonly ISqlDbContext _dbContext;

    public ProductRepository(ISqlDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Save(Product product)
    {
        var connection = (SqlConnection)_dbContext.GetConnection();

        // Raw SQL targeting your schema (prd)
        const string query = "INSERT INTO prd.Products (Code, Name, Price, Stock) VALUES (@Code, @Name, @Price, @Stock);";

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Code", product.Code);
        command.Parameters.AddWithValue("@Name", product.Name);
        command.Parameters.AddWithValue("@Price", product.Price);
        command.Parameters.AddWithValue("@Stock", product.Stock);

        command.ExecuteNonQuery();
    }

    public IEnumerable<Product> GetAll()
    {
        var products = new List<Product>();
        var connection = (SqlConnection)_dbContext.GetConnection();
        const string query = "SELECT Id, Code, Name, Price, Stock FROM prd.Products;";

        using var command = new SqlCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            products.Add(new Product
            {
                Id = reader.GetInt32(0),
                Code = reader.GetString(1),
                Name = reader.GetString(2),
                Price = reader.GetDecimal(3),
                Stock = reader.GetInt32(4)
            });
        }

        return products;
    }
}