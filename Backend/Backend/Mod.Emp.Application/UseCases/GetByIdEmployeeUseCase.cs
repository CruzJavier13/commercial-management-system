using Mod.Emp.Application.DTOs;
using Mod.Emp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Emp.Application.UseCases
{
    public class GetByIdEmployeeUseCase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public GetByIdEmployeeUseCase(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        }

        public async Task<GetEmployeeDto?> ExecuteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID del empleado proporcionado no es válido.");
            }

            var emp = await _employeeRepository.GetByIdAsync(id);

            if (emp == null)
            {
                return null;
            }

            return new GetEmployeeDto
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
            };
        }
    }
}
