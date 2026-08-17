using Mod.Emp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Emp.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(LoginResponseDto responseDto);
    }
}
