using CommercialSystem.Shared.Domain.Repositories;
using Microsoft.Data.SqlClient;
using Mod.Billing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Mod.Billing.Infrastructure.Persistence
{
    public class InvoiceRepository : IWriteOnlyRepository<Invoice>, IReadOnlyRepository<Invoice>
    {
        private readonly string _connectionString;

        public InvoiceRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Invoice>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Invoice?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("bil.sp_Invoice_GetById", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            var invoice = new Invoice
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                InvoiceNumber = reader.GetString(reader.GetOrdinal("InvoiceNumber")),
                OrderId = reader.GetInt32(reader.GetOrdinal("OrderId")),
                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                EmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                TaxAmount = reader.GetDecimal(reader.GetOrdinal("TaxAmount")),
                SubTotalAmount = reader.GetDecimal(reader.GetOrdinal("SubTotalAmount")),
                TotalBilled = reader.GetDecimal(reader.GetOrdinal("TotalBilled")),
                PaymentMethod = reader.GetString(reader.GetOrdinal("PaymentMethod")),
                InvoiceDate = reader.GetDateTime(reader.GetOrdinal("InvoiceDate"))
            };

            await reader.NextResultAsync();
            while (await reader.ReadAsync())
            {
                var detail = new InvoiceDetail
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    InvoiceId = reader.GetInt32(reader.GetOrdinal("InvoiceId")),
                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                    PriceBilled = reader.GetDecimal(reader.GetOrdinal("PriceBilled")),
                    TaxRate = reader.GetDecimal(reader.GetOrdinal("TaxRate"))
                };

                invoice.Details.Add(detail);
            }

            return invoice;
        }

        public Task SaveAsync(Invoice t)
        {
            throw new NotImplementedException();
        }

        public Task<Invoice> UpdateAsync(Invoice t, Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
