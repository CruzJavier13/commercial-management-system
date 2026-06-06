using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Emp.Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int RoleId { get; set; }
        public string? SystemUsername { get; set; }
        public string? PasswordHash { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual Employee Employee { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}
