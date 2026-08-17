using Microsoft.AspNetCore.Mvc;
using Mod.Emp.Application.DTOs;
using Mod.Emp.Application.UseCases.Auth;

namespace Commerce.API.Controllers.Auth
{
    [ApiController]
    [Route("api/login")]
    public class AuthController: BaseApiController
    {
            private readonly LoginUseCase _loginUseCase;

            public AuthController(LoginUseCase loginUseCase) => _loginUseCase = loginUseCase;

            [HttpPost]
            public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
            {
                try
                {
                    var result = await _loginUseCase.ExecuteAsync(request);

                    return Ok(new
                    {
                        Success = true,
                        Message = "Login exitoso",
                        Data = result 
                    });
                }
                catch (UnauthorizedAccessException ex)
                {
                    return Unauthorized(new { Success = false, Message = ex.Message });
                }
            }
    }
}
