using Mod.Emp.Application.DTOs;
using Mod.Emp.Application.Interfaces;
using Mod.Emp.Domain.Entities;
using Mod.Emp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Emp.Application.UseCases.Auth
{
    public class LoginUseCase
    {
        private readonly ISessionRepository<LoginRequestDto, LoginResponseDto> _sessionRepository;
        private readonly ITokenService _tokenService; 

        public LoginUseCase(
            ISessionRepository<LoginRequestDto, LoginResponseDto> sessionRepository,
            ITokenService tokenService)
        {
            _sessionRepository = sessionRepository;
            _tokenService = tokenService;
        }

        public async Task<LoginResponseDto> ExecuteAsync(LoginRequestDto request)
        {
            var responseDto = await _sessionRepository.ValidateCredentialsAsync(request);

            if (responseDto == null)
            {
                throw new UnauthorizedAccessException("Usuario o contraseña incorrectos.");
            }

            string jwtToken = _tokenService.GenerateToken(responseDto);

            responseDto.Token = jwtToken;

            return responseDto;
        }

    }
}
