using System.Data;
using Microsoft.Data.SqlClient;
using CommercialSystem.Shared.Persistence.Database;
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

    public async Task<int> SaveAsync(Product product)
    {
        using var connection = (SqlConnection)_dbContext.CreateConnection();


        using var command = new SqlCommand("prd.usp_Products_Save", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)
        { Value = product.Id > 0 ? product.Id : DBNull.Value });

        command.Parameters.Add(new SqlParameter("@ProductCode", SqlDbType.VarChar, 50) { Value = product.ProductCode });
        command.Parameters.Add(new SqlParameter("@CategoryId", SqlDbType.Int) { Value = product.CategoryId });
        command.Parameters.Add(new SqlParameter("@SupplierId", SqlDbType.Int) { Value = product.SupplierId });
        command.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 150) { Value = product.Name });
        command.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 500) { Value = (object?)product.Description ?? DBNull.Value });
        command.Parameters.Add(new SqlParameter("@BasePrice", SqlDbType.Decimal) { Value = product.BasePrice, Precision = 18, Scale = 2 });
        command.Parameters.Add(new SqlParameter("@IsVirtualService", SqlDbType.Bit) { Value = product.IsVirtualService });
        command.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit) { Value = product.IsActive });

        await connection.OpenAsync();


        var result = await command.ExecuteScalarAsync();

        if (result != null && result != DBNull.Value)
        {
            return Convert.ToInt32(result);
        }

        throw new InvalidOperationException("El procedimiento almacenado 'prd.usp_Products_Save' no retornó un identificador válido.");
    }

    public async Task<int> UpdateAsync(Product product)
    {
        return await SaveAsync(product);
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        using var connection = (SqlConnection)_dbContext.CreateConnection();
        const string query = @"SELECT Id, ProductCode, CategoryId, SupplierId, Name, Description, BasePrice, IsVirtualService, IsActive 
                               FROM prd.Products WHERE Id = @Id;";

        using var command = new SqlCommand(query, connection);
        command.CommandType = CommandType.Text;
        command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Product
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                ProductCode = reader.GetString(reader.GetOrdinal("ProductCode")),
                CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                SupplierId = reader.GetInt32(reader.GetOrdinal("SupplierId")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                BasePrice = reader.GetDecimal(reader.GetOrdinal("BasePrice")),
                IsVirtualService = reader.GetBoolean(reader.GetOrdinal("IsVirtualService")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
            };
        }

        return null;
    }


    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        var products = new List<Product>();
        using var connection = (SqlConnection)_dbContext.CreateConnection();
        const string query = @"SELECT Id, ProductCode, CategoryId, SupplierId, Name, Description, BasePrice, IsVirtualService, IsActive 
                               FROM prd.Products;";

        using var command = new SqlCommand(query, connection);
        command.CommandType = CommandType.Text;

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            products.Add(new Product
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                ProductCode = reader.GetString(reader.GetOrdinal("ProductCode")),
                CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                SupplierId = reader.GetInt32(reader.GetOrdinal("SupplierId")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                BasePrice = reader.GetDecimal(reader.GetOrdinal("BasePrice")),
                IsVirtualService = reader.GetBoolean(reader.GetOrdinal("IsVirtualService")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
            });
        }

        return products;
    }

    public async Task<int> DeleteAsync(int id)
    {
        using var connection = (SqlConnection)_dbContext.CreateConnection();
        const string query = "UPDATE prd.Products SET IsActive = 0 WHERE Id = @Id; SELECT @Id;";

        using var command = new SqlCommand(query, connection);
        command.CommandType = CommandType.Text;
        command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });

        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();

        return result != null ? Convert.ToInt32(result) : 0;
    }
}