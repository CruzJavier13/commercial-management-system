using Mod.Customers.Application.DTOs;
using Mod.Customers.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Customers.Application.UseCases
{
    public class GetByIdCustomerUseCase
    {
        private readonly ICustomerRepository _customerRepository;

        public GetByIdCustomerUseCase(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        public async Task<GetCustomerDto?> ExecuteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID del cliente proporcionado no es válido.");
            }

            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
            {
                return null;
            }

            return new GetCustomerDto
            {
                Id = customer.Id,
                CustomerCode = customer.CustomerCode,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                IdentificationNumber = customer.IdentificationNumber,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address,
                IsActive = customer.IsActive,
                CreatedAt = customer.CreatedAt
            };
        }
    }
}
