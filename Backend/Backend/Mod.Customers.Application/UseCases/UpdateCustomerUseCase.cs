using Mod.Customers.Application.DTOs;
using Mod.Customers.Domain.Entities;
using Mod.Customers.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Customers.Application.UseCases
{
    public class UpdateCustomerUseCase
    {
        private readonly ICustomerRepository _customerRepository;

        public UpdateCustomerUseCase(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        public async Task ExecuteAsync(int id, UpdateCustomerDto command)
        {

            if (id <= 0)
                throw new ArgumentException("El ID del cliente proporcionado no es válido.");

            if (string.IsNullOrWhiteSpace(command.CustomerCode))
                throw new ArgumentException("El código del cliente es obligatorio.");

            if (string.IsNullOrWhiteSpace(command.FirstName) || string.IsNullOrWhiteSpace(command.LastName))
                throw new ArgumentException("El nombre y apellido del cliente son obligatorios.");

            if (string.IsNullOrWhiteSpace(command.IdentificationNumber))
                throw new ArgumentException("El número de identificación (Cédula/RUC) es obligatorio.");

            if (!string.IsNullOrWhiteSpace(command.Email) && !command.Email.Contains("@"))
                throw new ArgumentException("El formato del correo electrónico proporcionado no es válido.");

            var customer = new Customer
            {
                Id = id, 
                CustomerCode = command.CustomerCode.Trim().ToUpper(),
                FirstName = command.FirstName.Trim(),
                LastName = command.LastName.Trim(),
                IdentificationNumber = command.IdentificationNumber.Trim().ToUpper(),
                Email = command.Email?.Trim().ToLower() ?? string.Empty,
                PhoneNumber = command.PhoneNumber?.Trim() ?? string.Empty,
                Address = command.Address?.Trim() ?? string.Empty,
                IsActive = true 
            };

            await _customerRepository.UpdateAsync(customer, id);
        }
    }
}
