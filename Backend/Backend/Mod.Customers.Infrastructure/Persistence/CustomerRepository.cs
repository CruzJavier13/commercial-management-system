using Microsoft.Data.SqlClient;
using Mod.Customers.Domain.Entities;
using Mod.Customers.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Mod.Customers.Infrastructure.Persistence
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }
        public async Task SaveAsync(Customer t)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("cus.sp_Customer_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@CustomerCode", t.CustomerCode);
            command.Parameters.AddWithValue("@FirstName", t.FirstName);
            command.Parameters.AddWithValue("@LastName", t.LastName);
            command.Parameters.AddWithValue("@IdentificationNumber", t.IdentificationNumber);
            command.Parameters.AddWithValue("@Email", t.Email);
            command.Parameters.AddWithValue("@PhoneNumber", t.PhoneNumber);
            command.Parameters.AddWithValue("@Address", t.Address);
            command.Parameters.AddWithValue("@IsActive", t.IsActive);
            command.Parameters.AddWithValue("@CreatedAt", t.CreatedAt);

            await command.ExecuteNonQueryAsync();
        }
        public async Task<Customer> UpdateAsync(Customer t, int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("cus.sp_Customer_Update", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@CustomerCode", t.CustomerCode);
            command.Parameters.AddWithValue("@FirstName", t.FirstName);
            command.Parameters.AddWithValue("@LastName", t.LastName);
            command.Parameters.AddWithValue("@IdentificationNumber", t.IdentificationNumber);
            command.Parameters.AddWithValue("@Email", t.Email);
            command.Parameters.AddWithValue("@PhoneNumber", t.PhoneNumber);
            command.Parameters.AddWithValue("@Address", t.Address);
            command.Parameters.AddWithValue("@IsActive", t.IsActive);

            int rowsAffected = await command.ExecuteNonQueryAsync();

            if (rowsAffected == 0)
            {
                throw new KeyNotFoundException($"No se encontró ningún cliente con el ID {id} para actualizar.");
            }

            return t;
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("cus.sp_Customer_Delete", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", id);

            return await command.ExecuteNonQueryAsync();
        }
        public async Task<Customer?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("cus.sp_Customer_GetById", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return new Customer
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                CustomerCode = reader.GetString(reader.GetOrdinal("CustomerCode")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                IdentificationNumber = reader.GetString(reader.GetOrdinal("IdentificationNumber")),
                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? string.Empty : reader.GetString(reader.GetOrdinal("Email")),
                PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? string.Empty : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? string.Empty : reader.GetString(reader.GetOrdinal("Address")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
            };
        }
        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            var customers = new List<Customer>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("cus.sp_Customer_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var customer = new Customer
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    CustomerCode = reader.GetString(reader.GetOrdinal("CustomerCode")),
                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                    IdentificationNumber = reader.GetString(reader.GetOrdinal("IdentificationNumber")),
                    Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? string.Empty : reader.GetString(reader.GetOrdinal("Email")),
                    PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? string.Empty : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                    Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? string.Empty : reader.GetString(reader.GetOrdinal("Address")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                };

                customers.Add(customer);
            }

            return customers;
        }
    }
}
