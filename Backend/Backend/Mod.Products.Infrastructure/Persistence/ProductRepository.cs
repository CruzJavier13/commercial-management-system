using System.Data;
using Microsoft.Data.SqlClient;
using CommercialSystem.Shared.Persistence.Database;
using Mod.Products.Domain.Entities;
using Mod.Products.Domain.Repositories;

namespace Mod.Products.Infrastructure.Persistence;

public class ProductRepository : IProductRepository
{
    private readonly string _connectionString;

    public ProductRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }
    public async Task SaveAsync(Product product)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand("prd.usp_Products_Save", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = product.Id > 0 ? product.Id : DBNull.Value });
        command.Parameters.Add(new SqlParameter("@ProductCode", SqlDbType.VarChar, 50) { Value = product.ProductCode });
        command.Parameters.Add(new SqlParameter("@CategoryId", SqlDbType.Int) { Value = product.CategoryId });
        command.Parameters.Add(new SqlParameter("@SupplierId", SqlDbType.Int) { Value = product.SupplierId });
        command.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 150) { Value = product.Name });
        command.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 500) { Value = (object?)product.Description ?? DBNull.Value });
        command.Parameters.Add(new SqlParameter("@BasePrice", SqlDbType.Decimal) { Value = product.BasePrice, Precision = 18, Scale = 2 });
        command.Parameters.Add(new SqlParameter("@IsVirtualService", SqlDbType.Bit) { Value = product.IsVirtualService });
        command.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit) { Value = product.IsActive });

        command.Parameters.Add(new SqlParameter("@HealthRegisterNumber", SqlDbType.VarChar, 50)
        { Value = (object?)product.MedicineAttributes?.HealthRegisterNumber ?? DBNull.Value });
        command.Parameters.Add(new SqlParameter("@ActiveIngredient", SqlDbType.VarChar, 150)
        { Value = (object?)product.MedicineAttributes?.ActiveIngredient ?? DBNull.Value });
        command.Parameters.Add(new SqlParameter("@ExpirationDateRequired", SqlDbType.Bit)
        { Value = product.MedicineAttributes != null ? product.MedicineAttributes.ExpirationDateRequired : DBNull.Value });
        command.Parameters.Add(new SqlParameter("@RequiresPrescription", SqlDbType.Bit)
        { Value = product.MedicineAttributes != null ? product.MedicineAttributes.RequiresPrescription : DBNull.Value });

        command.Parameters.Add(new SqlParameter("@Brand", SqlDbType.VarChar, 100)
        { Value = (object?)product.DeviceAttributes?.Brand ?? DBNull.Value });
        command.Parameters.Add(new SqlParameter("@Model", SqlDbType.VarChar, 100)
        { Value = (object?)product.DeviceAttributes?.Model ?? DBNull.Value });
        command.Parameters.Add(new SqlParameter("@SerialNumberOrIMEI", SqlDbType.VarChar, 100)
        { Value = (object?)product.DeviceAttributes?.SerialNumberOrIMEI ?? DBNull.Value });
        command.Parameters.Add(new SqlParameter("@WarrantyPeriodMonths", SqlDbType.Int)
        { Value = product.DeviceAttributes != null ? product.DeviceAttributes.WarrantyPeriodMonths : DBNull.Value });

        var result = await command.ExecuteScalarAsync();

        if (result != null && result != DBNull.Value)
        {
             Convert.ToInt32(result);
        }

        throw new InvalidOperationException("El procedimiento almacenado 'prd.usp_Products_Save' no retornó un identificador válido.");
    }

    public async Task UpdateAsync(Product product)
    {
         await SaveAsync(product);
    }
    public async Task<int> DeleteAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand("prd.usp_Product_Delete", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@Id", id);

        return await command.ExecuteNonQueryAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand("prd.usp_Products_GetById", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return MapFromReader(reader);
        }

        return null;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        var products = new List<Product>();

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand("prd.usp_Products_GetAll", connection);
        command.CommandType = CommandType.StoredProcedure;

        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            products.Add(MapFromReader(reader));
        }

        return products;
    }

    private static Product MapFromReader(SqlDataReader reader)
    {
        var product = new Product
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

        if (!reader.IsDBNull(reader.GetOrdinal("HealthRegisterNumber")))
        {
            product.MedicineAttributes = new MedicineAttributes
            {
                HealthRegisterNumber = reader.GetString(reader.GetOrdinal("HealthRegisterNumber")),
                ActiveIngredient = reader.IsDBNull(reader.GetOrdinal("ActiveIngredient")) ? null : reader.GetString(reader.GetOrdinal("ActiveIngredient")),
                ExpirationDateRequired = reader.GetBoolean(reader.GetOrdinal("ExpirationDateRequired")),
                RequiresPrescription = reader.GetBoolean(reader.GetOrdinal("RequiresPrescription"))
            };
        }

        if (!reader.IsDBNull(reader.GetOrdinal("Brand")))
        {
            product.DeviceAttributes = new DeviceAttributes
            {
                Brand = reader.GetString(reader.GetOrdinal("Brand")),
                Model = reader.IsDBNull(reader.GetOrdinal("Model")) ? null : reader.GetString(reader.GetOrdinal("Model")),
                SerialNumberOrIMEI = reader.IsDBNull(reader.GetOrdinal("SerialNumberOrIMEI")) ? null : reader.GetString(reader.GetOrdinal("SerialNumberOrIMEI")),
                WarrantyPeriodMonths = reader.GetInt32(reader.GetOrdinal("WarrantyPeriodMonths"))
            };
        }

        return product;
    }

    public Task<Product> UpdateAsync(Product t, int id)
    {
        throw new NotImplementedException();
    }
}