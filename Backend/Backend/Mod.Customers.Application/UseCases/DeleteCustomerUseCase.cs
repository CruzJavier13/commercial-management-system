using Mod.Customers.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Customers.Application.UseCases
{
    public class DeleteCustomerUseCase
    {
        private readonly ICustomerRepository _customerRepository;

        public DeleteCustomerUseCase(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task ExecuteAsync(int customerId)
        {
            if (customerId <= 0)
            {
                throw new ArgumentException("El identificador del cliente proporcionado no es válido.", nameof(customerId));
            }

            await _customerRepository.DeleteAsync(customerId);
        }
    }
}
