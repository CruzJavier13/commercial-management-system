using Mod.Emp.Application.DTOs;
using Mod.Emp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Emp.Application.UseCases
{
    public class GetAllEmployeeUseCase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public GetAllEmployeeUseCase(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        }

        public async Task<IEnumerable<GetEmployeeDto>> ExecuteAsync()
        {

            var employees = await _employeeRepository.GetAllAsync();

            if (employees == null)
            {
                return Enumerable.Empty<GetEmployeeDto>();
            }

            return employees.Select(emp => new GetEmployeeDto
            {
                Id = emp.Id,
                EmployeeCode = emp.EmployeeCode,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                IdentificationNumber = emp.IdentificationNumber,
                SocialSecurity = emp.SocialSecurity,
                Phone = emp.Phone,
                Address = emp.Address,
                IsActive = emp.IsActive,
                CreatedAt = emp.CreatedAt
            }).ToList();
        }
    }
}
