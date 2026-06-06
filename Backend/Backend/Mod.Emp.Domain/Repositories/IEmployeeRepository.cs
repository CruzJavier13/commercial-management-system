using Mod.Emp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Emp.Domain.Repositories
{
    public interface IEmployeeRepository
    {
        Task<int> SaveAsync(
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
       string? passwordHash);

        Task<int> UpdateAsync(
        int? id,
        string employeeCode,
        string firstName,
        string lastName,
        string identificationNumber,
        string? socialSecurity,
        string phone,
        string? address,
        bool isActive,
        int RoleId,
        string SystemUsername,
        string PasswordHash);
        Task<int> DeleteAsync(int id);
        Task<int> GetByIdAsync(int id);
        Task<IEnumerable<Employee>> GetAllAsync();
    }
}
