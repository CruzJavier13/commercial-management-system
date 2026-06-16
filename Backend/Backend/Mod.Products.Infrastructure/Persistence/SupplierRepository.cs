using Microsoft.Data.SqlClient;
using Mod.Products.Domain.Entities;
using Mod.Products.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Mod.Products.Infrastructure.Persistence;

public class SupplierRepository : ISupplierRepository
{
    private readonly string _connectionString;

    public SupplierRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public async Task SaveAsync(Supplier t)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand("prd.usp_Suppliers_Upsert", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Id", DBNull.Value);
        command.Parameters.AddWithValue("@SupplierCode", t.SupplierCode);
        command.Parameters.AddWithValue("@CompanyName", t.CompanyName);
        command.Parameters.AddWithValue("@TaxIdentification", t.TaxIdentification);
        command.Parameters.AddWithValue("@Email", t.Email ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@PhoneNumber", t.PhoneNumber ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Address", t.Address ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@IsActive", t.IsActive);

        var result = await command.ExecuteScalarAsync();

        if (result != null && result != DBNull.Value)
        {
            t.Id = Convert.ToInt32(result);
            return;
        }

        throw new InvalidOperationException("El procedimiento 'prd.usp_Suppliers_Upsert' no retornó un identificador válido.");
    }

    public async Task<Supplier> UpdateAsync(Supplier t, int id)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand("prd.usp_Suppliers_Upsert", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Id", id);
        command.Parameters.AddWithValue("@SupplierCode", t.SupplierCode);
        command.Parameters.AddWithValue("@CompanyName", t.CompanyName);
        command.Parameters.AddWithValue("@TaxIdentification", t.TaxIdentification);
        command.Parameters.AddWithValue("@Email", t.Email ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@PhoneNumber", t.PhoneNumber ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Address", t.Address ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@IsActive", t.IsActive);

        var result = await command.ExecuteScalarAsync();

        if (result == null || result == DBNull.Value)
        {
            throw new KeyNotFoundException($"No se pudo actualizar el proveedor. Confirme si el ID {id} es correcto.");
        }

        t.Id = id;
        return t;
    }

    public async Task<Supplier?> GetByIdAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand("prd.usp_Suppliers_GetById", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return MapFromReader(reader);
        }

        return null;
    }

    public async Task<IEnumerable<Supplier>> GetAllAsync()
    {
        var list = new List<Supplier>();

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand("prd.usp_Suppliers_GetAll", connection);
        command.CommandType = CommandType.StoredProcedure;

        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(MapFromReader(reader));
        }

        return list;
    }

    public async Task<int> DeleteAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand("prd.usp_Suppliers_Delete", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@Id", id);

        var result = await command.ExecuteScalarAsync();
        return result != null ? Convert.ToInt32(result) : 0;
    }

    private static Supplier MapFromReader(SqlDataReader reader)
    {
        return new Supplier
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            SupplierCode = reader.GetString(reader.GetOrdinal("SupplierCode")),
            CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")),
            TaxIdentification = reader.GetString(reader.GetOrdinal("TaxIdentification")),

            Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
            PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? null : reader.GetString(reader.GetOrdinal("PhoneNumber")),
            Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
        };
    }
}
