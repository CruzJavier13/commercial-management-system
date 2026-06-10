using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Customers.Application.DTOs
{
    public class GetCustomerDto
    {
        public int Id { get; set; }

        public string CustomerCode { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? IdentificationNumber { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
