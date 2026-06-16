using Microsoft.Data.SqlClient;
using Mod.Inventory.Domain.Entities;
using Mod.Inventory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Mod.Inventory.Infrastructure.Persistence
{
    public class StockMovementRepository : IStockMovementRepository
    {
        private readonly string _connectionString;

        public StockMovementRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task SaveAsync(StockMovement movement)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("inv.usp_StockMovements_Register", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ProductId", movement.ProductId);
            command.Parameters.AddWithValue("@SupplierId", movement.SupplierId ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@MovementType", movement.MovementType);
            command.Parameters.AddWithValue("@Quantity", movement.Quantity);
            command.Parameters.AddWithValue("@UnitCost", movement.UnitCost);
            command.Parameters.AddWithValue("@Concept", movement.Concept);
            command.Parameters.AddWithValue("@ReferenceDocument", movement.ReferenceDocument ?? (object)DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<StockMovement>> GetAllAsync()
        {
            var list = new List<StockMovement>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("inv.usp_StockMovements_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(MapFromReader(reader));
            }

            return list;
        }

        public async Task<IEnumerable<StockMovement>> GetByProductIdAsync(int productId)
        {
            var list = new List<StockMovement>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("inv.usp_StockMovements_GetByProductId", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ProductId", productId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(MapFromReader(reader));
            }

            return list;
        }

        private static StockMovement MapFromReader(SqlDataReader reader)
        {
            return new StockMovement
            {
                Id = reader.GetInt64(reader.GetOrdinal("Id")), 
                ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                SupplierId = reader.IsDBNull(reader.GetOrdinal("SupplierId")) ? null : reader.GetInt32(reader.GetOrdinal("SupplierId")),
                MovementType = reader.GetString(reader.GetOrdinal("MovementType")),
                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                UnitCost = reader.GetDecimal(reader.GetOrdinal("UnitCost")),
                TotalCost = reader.GetDecimal(reader.GetOrdinal("TotalCost")), 
                Concept = reader.GetString(reader.GetOrdinal("Concept")),
                ReferenceDocument = reader.IsDBNull(reader.GetOrdinal("ReferenceDocument")) ? null : reader.GetString(reader.GetOrdinal("ReferenceDocument")),
                MovementDate = reader.GetDateTime(reader.GetOrdinal("MovementDate"))
            };
        }

        public Task<StockMovement?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<StockMovement> UpdateAsync(StockMovement t, int id)
        {
            throw new NotImplementedException();
        }
    }
}
