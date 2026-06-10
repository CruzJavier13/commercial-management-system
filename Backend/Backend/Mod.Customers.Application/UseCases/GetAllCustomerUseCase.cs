using Mod.Customers.Application.DTOs;
using Mod.Customers.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Customers.Application.UseCases
{
    public class GetAllCustomerUseCase
    {
        private readonly ICustomerRepository _customerRepository;

        public GetAllCustomerUseCase(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<GetCustomerDto>> ExecuteAsync()
        {
            var customers = await _customerRepository.GetAllAsync();

            if (customers == null)
            {
                return Enumerable.Empty<GetCustomerDto>();
            }

            return customers.Select(customer => new GetCustomerDto
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
            }).ToList();
        }
    }
}
