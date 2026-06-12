using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Emp.Domain.Entities
{
    public class Session
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int RoleId { get; set; }
        public string SystemUsername { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public virtual Employee Employee { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}
