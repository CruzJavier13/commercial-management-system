using Mod.Emp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Emp.Application.UseCases
{
    public class DeleteEmployeeUseCase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public DeleteEmployeeUseCase(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        }

        public async Task ExecuteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID del empleado proporcionado no es válido.");
            }


            int rowsAffected = await _employeeRepository.DeleteAsync(id);

            if (rowsAffected == 0)
            {
                throw new KeyNotFoundException($"No se encontró ningún empleado activo con el ID {id}.");
            }
        }
    }
}
