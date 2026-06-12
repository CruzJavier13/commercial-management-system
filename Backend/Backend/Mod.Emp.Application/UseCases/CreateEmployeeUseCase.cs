using Mod.Emp.Application.DTOs;
using Mod.Emp.Domain.Entities;
using Mod.Emp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Emp.Application.UseCases
{
    public class CreateEmployeeUseCase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public CreateEmployeeUseCase(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        }

        public async Task ExecuteAsync(CreateEmployeeDto command)
        {
            if (string.IsNullOrWhiteSpace(command.EmployeeCode))
                throw new ArgumentException("El código del empleado es obligatorio.");

            if (string.IsNullOrWhiteSpace(command.FirstName) || string.IsNullOrWhiteSpace(command.LastName))
                throw new ArgumentException("El nombre y apellido del empleado son obligatorios.");

            if (string.IsNullOrWhiteSpace(command.IdentificationNumber))
                throw new ArgumentException("El número de cédula o identificación es obligatorio.");

            if (string.IsNullOrWhiteSpace(command.Phone))
                throw new ArgumentException("El número de teléfono de contacto es obligatorio.");

            if (command.RoleId <= 0)
                throw new ArgumentException("Debe asignar un rol válido de acceso al sistema.");

            if (string.IsNullOrWhiteSpace(command.SystemUsername))
                throw new ArgumentException("El nombre de usuario para el sistema es obligatorio.");

            if (string.IsNullOrWhiteSpace(command.PasswordHash))
                throw new ArgumentException("La contraseña de acceso no puede estar vacía.");


            var employee = new Employee
            {
                EmployeeCode = command.EmployeeCode.Trim().ToUpper(),
                FirstName = command.FirstName.Trim(),
                LastName = command.LastName.Trim(),
                IdentificationNumber = command.IdentificationNumber.Trim().ToUpper(),
                SocialSecurity = command.SocialSecurity?.Trim() ?? string.Empty,
                Phone = command.Phone.Trim(),
                Address = command.Address?.Trim() ?? string.Empty,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,

                SessionAuth = new Session
                {
                    RoleId = command.RoleId,
                    SystemUsername = command.SystemUsername.Trim().ToLower(),
                    PasswordHash = command.PasswordHash
                }
            };
            await _employeeRepository.SaveAsync(employee);
        }
    }
}
