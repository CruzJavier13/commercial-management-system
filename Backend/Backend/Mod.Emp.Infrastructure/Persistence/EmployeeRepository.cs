using CommercialSystem.Shared.Persistence.Database;
using Microsoft.Data.SqlClient;
using Mod.Emp.Domain.Entities;
using Mod.Emp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Mod.Emp.Infrastructure.Persistence
{
    public class EmployeeRepository: IEmployeeRepository
    {
        private readonly ISqlDbContext _dbContext;

        public EmployeeRepository(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Employee>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> SaveAsync(
            int? id,
            string employeeCode,
            string firstName,
            string lastName,
            string identificationNumber,
            string? socialSecurity,
            string phone,
            string? address,
            bool isActive,
            int roleId,
            string systemUsername,
            string? passwordHash)
        {

            using var connection = (SqlConnection)_dbContext.CreateConnection();

            using var command = new SqlCommand("emp.usp_Employees_Save", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@Id", id.HasValue && id.Value > 0 ? id.Value : DBNull.Value));
            command.Parameters.Add(new SqlParameter("@EmployeeCode", employeeCode));
            command.Parameters.Add(new SqlParameter("@FirstName", firstName));
            command.Parameters.Add(new SqlParameter("@LastName", lastName));
            command.Parameters.Add(new SqlParameter("@IdentificationNumber", identificationNumber));
            command.Parameters.Add(new SqlParameter("@SocialSecurity", (object?)socialSecurity ?? DBNull.Value));
            command.Parameters.Add(new SqlParameter("@Phone", phone));
            command.Parameters.Add(new SqlParameter("@Address", (object?)address ?? DBNull.Value));
            command.Parameters.Add(new SqlParameter("@IsActive", isActive));
            command.Parameters.Add(new SqlParameter("@RoleId", roleId));
            command.Parameters.Add(new SqlParameter("@SystemUsername", systemUsername));
            command.Parameters.Add(new SqlParameter("@PasswordHash", (object?)passwordHash ?? DBNull.Value));

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();

            if (result != null && result != DBNull.Value)
            {
                return Convert.ToInt32(result);
            }

            throw new InvalidOperationException("El procedimiento almacenado 'emp.usp_Employees_Save' no retornó un ID válido.");
        }

        public Task<int> UpdateAsync(int? id, string employeeCode, string firstName, string lastName, string identificationNumber, string? socialSecurity, string phone, string? address, bool isActive, int RoleId, string SystemUsername, string PasswordHash)
        {
            throw new NotImplementedException();
        }
    }
}
