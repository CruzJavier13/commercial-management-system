using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Customers.Domain.Exceptions
{
    public class BusinessRule: Exception
    {
        public BusinessRule(string message) : base(message) { }
    }
}
