using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Emp.Application.DTOs
{
    public class LoginResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Rol { get; set; }
        public string Token { get; set; }
    }
}
