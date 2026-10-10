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

        // Send an OTP / reset code to the user's phone number and prepare a reset token.
        Task SendPasswordResetCodeAsync(string phoneNumber);

        // Verify the OTP sent to the user's phone number.
        Task<bool> VerifyPasswordResetCodeAsync(string phoneNumber, string code);

        // Reset the user's password after successful OTP verification.
        Task ResetPasswordByPhoneAsync(string phoneNumber, string newPassword);
    }
}
