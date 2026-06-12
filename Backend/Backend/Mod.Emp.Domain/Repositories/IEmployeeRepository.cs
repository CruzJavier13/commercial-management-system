using CommercialSystem.Shared.Domain.Repositories;
using Mod.Emp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Emp.Domain.Repositories
{
    public interface IEmployeeRepository : IReadOnlyRepository<Employee>, IWriteOnlyRepository<Employee>
    {
        
    }
}
