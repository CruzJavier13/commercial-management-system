using Microsoft.Data.SqlClient;
using Mod.Emp.Application.DTOs;
using Mod.Emp.Domain.Entities;
using Mod.Emp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BCrypt.Net;

namespace Mod.Emp.Infrastructure.Persistence
{
    public class SessionRepository : ISessionRepository<LoginRequestDto, LoginResponseDto>
    {
        private readonly string _connectionString;

        public SessionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<LoginResponseDto> ValidateCredentialsAsync(LoginRequestDto request)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("emp.usp_Auth_Login", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Username", request.Username);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                string databaseHash = reader.GetString(reader.GetOrdinal("PasswordHash")).Trim();
                string hashFrescoEnMemoria = BCrypt.Net.BCrypt.HashPassword("cajero1");
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Username, databaseHash);

                if (!isPasswordValid) return null;

                int firstNameOrdinal = reader.GetOrdinal("FirstName");
                int lastNameOrdinal = reader.GetOrdinal("LastName");
                string fullName = $"{reader.GetString(firstNameOrdinal)} {reader.GetString(lastNameOrdinal)}";

                return new LoginResponseDto
                {
                    Id = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                    Nombre = fullName,
                    Rol = reader.GetString(reader.GetOrdinal("RoleName")),
                    Token = string.Empty
                };
            }

            return null;
        }
    }
}
