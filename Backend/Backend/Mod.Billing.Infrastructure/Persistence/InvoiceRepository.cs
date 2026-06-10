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
        public async Task<int> DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("bil.sp_Invoice_Delete", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", id);

            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected;
        }
        
        public async Task<IEnumerable<Invoice>> GetAllAsync()
        {
            var invoices = new List<Invoice>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("bil.sp_Invoice_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var invoice = new Invoice
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    InvoiceNumber = reader.GetString(reader.GetOrdinal("InvoiceNumber")),
                    //OrderId = reader.GetInt32(reader.GetOrdinal("OrderId")),
                    CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                    EmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                    TaxAmount = reader.GetDecimal(reader.GetOrdinal("TaxAmount")),
                    SubTotalAmount = reader.GetDecimal(reader.GetOrdinal("SubTotalAmount")),
                    TotalBilled = reader.GetDecimal(reader.GetOrdinal("TotalBilled")),
                    PaymentMethod = reader.GetString(reader.GetOrdinal("PaymentMethod")),
                    InvoiceDate = reader.GetDateTime(reader.GetOrdinal("InvoiceDate"))
                };

                invoices.Add(invoice);
            }

            return invoices;
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
                //OrderId = reader.GetInt32(reader.GetOrdinal("OrderId")),
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
        // Method to save an invoice along with its details using a stored procedure and table-valued parameter
        public async Task SaveAsync(Invoice entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var tableDetails = new DataTable();
            tableDetails.Columns.Add("ProductId", typeof(int));
            tableDetails.Columns.Add("Quantity", typeof(int));
            tableDetails.Columns.Add("PriceBilled", typeof(decimal));
            tableDetails.Columns.Add("TaxRate", typeof(decimal));

            foreach (var detail in entity.Details)
            {
                tableDetails.Rows.Add(
                    detail.ProductId,
                    detail.Quantity,
                    detail.PriceBilled,
                    detail.TaxRate
                );
            }


            using var command = new SqlCommand("bil.sp_Invoice_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@InvoiceNumber", entity.InvoiceNumber);
            //command.Parameters.AddWithValue("@OrderId", entity.OrderId);
            command.Parameters.AddWithValue("@CustomerId", entity.CustomerId);
            command.Parameters.AddWithValue("@EmployeeId", entity.EmployeeId);
            command.Parameters.AddWithValue("@TaxAmount", entity.TaxAmount);
            command.Parameters.AddWithValue("@SubTotalAmount", entity.SubTotalAmount);
            command.Parameters.AddWithValue("@TotalBilled", entity.TotalBilled);
            command.Parameters.AddWithValue("@PaymentMethod", entity.PaymentMethod);
            command.Parameters.AddWithValue("@InvoiceDate", entity.InvoiceDate);

            var pDetails = new SqlParameter("@Details", SqlDbType.Structured)
            {
                TypeName = "bil.InvoiceDetailType", 
                Value = tableDetails
            };
            command.Parameters.Add(pDetails);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<Invoice> UpdateAsync(Invoice t, int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var tableDetails = new DataTable();
            tableDetails.Columns.Add("ProductId", typeof(int));
            tableDetails.Columns.Add("Quantity", typeof(int));
            tableDetails.Columns.Add("PriceBilled", typeof(decimal));
            tableDetails.Columns.Add("TaxRate", typeof(decimal));

            foreach (var detail in t.Details)
            {
                tableDetails.Rows.Add(detail.ProductId, detail.Quantity, detail.PriceBilled, detail.TaxRate);
            }

            using var command = new SqlCommand("bil.sp_Invoice_Update", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", id);

            command.Parameters.AddWithValue("@InvoiceNumber", t.InvoiceNumber);
            //command.Parameters.AddWithValue("@OrderId", t.OrderId);
            command.Parameters.AddWithValue("@CustomerId", t.CustomerId);
            command.Parameters.AddWithValue("@EmployeeId", t.EmployeeId);
            command.Parameters.AddWithValue("@TaxAmount", t.TaxAmount);
            command.Parameters.AddWithValue("@SubTotalAmount", t.SubTotalAmount);
            command.Parameters.AddWithValue("@TotalBilled", t.TotalBilled);
            command.Parameters.AddWithValue("@PaymentMethod", t.PaymentMethod);
            command.Parameters.AddWithValue("@InvoiceDate", t.InvoiceDate);

            var pDetails = new SqlParameter("@Details", SqlDbType.Structured)
            {
                TypeName = "bil.InvoiceDetailType",
                Value = tableDetails
            };
            command.Parameters.Add(pDetails);

            int rowsAffected = await command.ExecuteNonQueryAsync();

            if (rowsAffected == 0)
            {
                throw new KeyNotFoundException($"No se encontró ninguna factura con el ID {id} para actualizar.");
            }

            return t;
        }
    }
}
