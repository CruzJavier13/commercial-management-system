using Mod.Emp.Application.DTOs;
using Mod.Emp.Domain.Entities;
using Mod.Emp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Emp.Application.UseCases
{
    public class UpdateEmployeeUseCase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public UpdateEmployeeUseCase(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        }

        public async Task ExecuteAsync(int id, UpdateEmployeeDto command)
        {
            if (id <= 0)
                throw new ArgumentException("El ID del empleado proporcionado no es válido.");

            if (string.IsNullOrWhiteSpace(command.EmployeeCode))
                throw new ArgumentException("El código del empleado es obligatorio.");

            if (string.IsNullOrWhiteSpace(command.FirstName) || string.IsNullOrWhiteSpace(command.LastName))
                throw new ArgumentException("El nombre y apellido del empleado son obligatorios.");

            if (string.IsNullOrWhiteSpace(command.IdentificationNumber))
                throw new ArgumentException("El número de cédula o identificación es obligatorio.");

            if (string.IsNullOrWhiteSpace(command.Phone))
                throw new ArgumentException("El número de teléfono de contacto es obligatorio.");

            var employee = new Employee
            {
                Id = id, 
                EmployeeCode = command.EmployeeCode.Trim().ToUpper(),
                FirstName = command.FirstName.Trim(),
                LastName = command.LastName.Trim(),
                IdentificationNumber = command.IdentificationNumber.Trim().ToUpper(),
                SocialSecurity = command.SocialSecurity?.Trim() ?? string.Empty,
                Phone = command.Phone.Trim(),
                Address = command.Address?.Trim() ?? string.Empty,
                IsActive = true 
            };

            await _employeeRepository.UpdateAsync(employee, id);
        }
    }
}
