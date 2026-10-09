using EduNova.Application.Features.Authentication.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduNova.Application.Contracts.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);

        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }
}
