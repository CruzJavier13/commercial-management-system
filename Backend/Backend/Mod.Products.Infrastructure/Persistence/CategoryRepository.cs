using Microsoft.Data.SqlClient;
using Mod.Products.Domain.Entities;
using Mod.Products.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Mod.Products.Infrastructure.Persistence
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string _connectionString;

        public CategoryRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }
        public async Task SaveAsync(Category t)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("prd.usp_Categories_Upsert", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", DBNull.Value);
            command.Parameters.AddWithValue("@CategoryCode", t.CategoryCode);
            command.Parameters.AddWithValue("@Name", t.Name);
            command.Parameters.AddWithValue("@Description", t.Description ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@IsActive", t.IsActive);

            var result = await command.ExecuteScalarAsync();

            if (result != null && result != DBNull.Value)
            {
                t.Id = Convert.ToInt32(result);
                return;
            }

            throw new InvalidOperationException("El procedimiento 'prd.usp_Categories_Upsert' no retornó un identificador válido.");
        }
        public async Task<Category> UpdateAsync(Category t, int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("prd.usp_Categories_Upsert", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@CategoryCode", t.CategoryCode);
            command.Parameters.AddWithValue("@Name", t.Name);
            command.Parameters.AddWithValue("@Description", t.Description ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@IsActive", t.IsActive);

            var result = await command.ExecuteScalarAsync();

            if (result == null || result == DBNull.Value)
            {
                throw new KeyNotFoundException($"No se pudo actualizar la categoría. Confirme si el ID {id} es correcto.");
            }

            t.Id = id;
            return t;
        }
        public async Task<Category?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("prd.usp_Categories_GetById", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Category
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    CategoryCode = reader.GetString(reader.GetOrdinal("CategoryCode")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")) 
                };
            }

            return null;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var list = new List<Category>();
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("prd.usp_Categories_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new Category
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    CategoryCode = reader.GetString(reader.GetOrdinal("CategoryCode")), 
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")) 
                });
            }

            return list;
        }


        public async Task<int> DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            const string query = "UPDATE prd.Categories SET IsActive = 0 WHERE Id = @Id; SELECT @@ROWCOUNT;";

            using var command = new SqlCommand(query, connection);
            command.CommandType = CommandType.Text;
            command.Parameters.AddWithValue("@Id", id);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();

            return result != null ? Convert.ToInt32(result) : 0;
        }
    }
}
