using Mod.Emp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Emp.Application.DTOs
{
    public class UpdateEmployeeDto
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string IdentificationNumber { get; set; } = string.Empty;
        public string? SocialSecurity { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string? Address { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? RoleId { get; set; }
        public string? SystemUsername { get; set; }
        public string? PasswordHash { get; set; }
    }
}
