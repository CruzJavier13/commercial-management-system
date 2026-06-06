using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Emp.Domain.Entities
{
    public class Session
    {
        public int Id { get; set; }
        public string RoleCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
