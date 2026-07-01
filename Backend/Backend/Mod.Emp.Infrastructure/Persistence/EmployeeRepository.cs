using CommercialSystem.Shared.Persistence.Database;
using Microsoft.Data.SqlClient;
using Mod.Emp.Domain.Entities;
using Mod.Emp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Text;

namespace Mod.Emp.Infrastructure.Persistence
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<int> DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("emp.usp_Employees_Delete", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", id);

            return await command.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            var employees = new List<Employee>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("emp.usp_Employees_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                employees.Add(new Employee
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    EmployeeCode = reader.GetString(reader.GetOrdinal("EmployeeCode")),
                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                    IdentificationNumber = reader.GetString(reader.GetOrdinal("IdentificationNumber")),
                    SocialSecurity = reader.IsDBNull(reader.GetOrdinal("SocialSecurity")) ? string.Empty : reader.GetString(reader.GetOrdinal("SocialSecurity")),
                    Phone = reader.GetString(reader.GetOrdinal("Phone")),
                    Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? string.Empty : reader.GetString(reader.GetOrdinal("Address")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                    BaseSalary = reader.IsDBNull(reader.GetOrdinal("BaseSalary")) ? 0m : reader.GetDecimal(reader.GetOrdinal("BaseSalary")),
                    RoleName = reader.GetString(reader.GetOrdinal("RoleName")),
                    RoleCode = reader.GetString(reader.GetOrdinal("RoleCode")),

                    RoleId = reader.IsDBNull(reader.GetOrdinal("RoleId")) ? null : reader.GetInt32(reader.GetOrdinal("RoleId")),
                    SystemUsername = reader.IsDBNull(reader.GetOrdinal("SystemUsername")) ? null : reader.GetString(reader.GetOrdinal("SystemUsername")),
                    PasswordHash = reader.IsDBNull(reader.GetOrdinal("PasswordHash")) ? null : reader.GetString(reader.GetOrdinal("PasswordHash"))
                });
            }

            return employees;
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("emp.usp_Employees_GetById", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync()) return null;

            return new Employee
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                EmployeeCode = reader.GetString(reader.GetOrdinal("EmployeeCode")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                IdentificationNumber = reader.GetString(reader.GetOrdinal("IdentificationNumber")), 
                SocialSecurity = reader.IsDBNull(reader.GetOrdinal("SocialSecurity")) ? string.Empty : reader.GetString(reader.GetOrdinal("SocialSecurity")), 
                Phone = reader.GetString(reader.GetOrdinal("Phone")), 
                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? string.Empty : reader.GetString(reader.GetOrdinal("Address")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                BaseSalary = reader.IsDBNull(reader.GetOrdinal("BaseSalary")) ? 0m : reader.GetDecimal(reader.GetOrdinal("BaseSalary")),
                RoleName = reader.GetString(reader.GetOrdinal("RoleName")),
                RoleCode = reader.GetString(reader.GetOrdinal("RoleCode")),

                RoleId = reader.IsDBNull(reader.GetOrdinal("RoleId")) ? null : reader.GetInt32(reader.GetOrdinal("RoleId")),
                SystemUsername = reader.IsDBNull(reader.GetOrdinal("SystemUsername")) ? null : reader.GetString(reader.GetOrdinal("SystemUsername")),
                PasswordHash = reader.IsDBNull(reader.GetOrdinal("PasswordHash")) ? null : reader.GetString(reader.GetOrdinal("PasswordHash"))
            };
        }

        public async Task SaveAsync(Employee t)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("emp.usp_Employees_Save", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", DBNull.Value);
            command.Parameters.AddWithValue("@EmployeeCode", t.EmployeeCode);
            command.Parameters.AddWithValue("@FirstName", t.FirstName);
            command.Parameters.AddWithValue("@LastName", t.LastName);
            command.Parameters.AddWithValue("@IdentificationNumber", t.IdentificationNumber);
            command.Parameters.AddWithValue("@SocialSecurity", t.SocialSecurity ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Phone", t.Phone);
            command.Parameters.AddWithValue("@Address", t.Address ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@IsActive", t.IsActive);
            command.Parameters.AddWithValue("@BaseSalary", t.BaseSalary);

            command.Parameters.AddWithValue("@RoleId", t.SessionAuth?.RoleId ?? 0);
            command.Parameters.AddWithValue("@SystemUsername", t.SessionAuth?.SystemUsername ?? string.Empty);
            command.Parameters.AddWithValue("@PasswordHash", t.SessionAuth?.PasswordHash ?? (object)DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<Employee> UpdateAsync(Employee t, int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("emp.usp_Employees_Save", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@EmployeeCode", t.EmployeeCode);
            command.Parameters.AddWithValue("@FirstName", t.FirstName);
            command.Parameters.AddWithValue("@LastName", t.LastName);
            command.Parameters.AddWithValue("@IdentificationNumber", t.IdentificationNumber);
            command.Parameters.AddWithValue("@SocialSecurity", t.SocialSecurity ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Phone", t.Phone);
            command.Parameters.AddWithValue("@Address", t.Address ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@IsActive", t.IsActive);
            command.Parameters.AddWithValue("@BaseSalary", t.BaseSalary);

            command.Parameters.AddWithValue("@RoleId", t.SessionAuth?.RoleId ?? 0);
            command.Parameters.AddWithValue("@SystemUsername", t.SessionAuth?.SystemUsername ?? string.Empty);
            command.Parameters.AddWithValue("@PasswordHash", t.SessionAuth?.PasswordHash ?? (object)DBNull.Value);

            await command.ExecuteNonQueryAsync();
            return t;
        }
    }
}
