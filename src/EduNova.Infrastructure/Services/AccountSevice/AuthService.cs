using EduNova.Application.Contracts.Services;
using EduNova.Application.Features.Authentication.DTOs;
using EduNova.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EduNova.Infrastructure.Services.AccountSevice
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly Application.Common.Caching.ICacheService _cacheService;

        public AuthService(
            UserManager<AppUser> userManager,
            ITokenService tokenService,
            Application.Common.Caching.ICacheService cacheService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _cacheService = cacheService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);

            if (existingUser is not null)
            {
                throw new InvalidOperationException(
                    "User with this email already exists.");
            }

            var user = new AppUser
            {
                DisplayName = dto.DisplayName,
                Email = dto.Email,
                UserName = dto.Email
            };

            var result = await _userManager.CreateAsync(
                user,
                dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(e => e.Description));

                throw new InvalidOperationException(errors);
            }

            var token = await _tokenService.CreateTokenAsync(user);

            return new AuthResponseDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = token
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user is null)
            {
                throw new UnauthorizedAccessException(
                    "Invalid email or password.");
            }

            var passwordValid = await _userManager.CheckPasswordAsync(
                user,
                dto.Password);

            if (!passwordValid)
            {
                throw new UnauthorizedAccessException(
                    "Invalid email or password.");
            }

            var token = await _tokenService.CreateTokenAsync(user);

            return new AuthResponseDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = token
            };
        }

        public async Task SendPasswordResetCodeAsync(string phoneNumber)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

            if (user is null)
            {
                // do not reveal user does not exist for security; return silently
                return;
            }

            // generate a 4-digit numeric code
            var rng = new Random();
            var code = rng.Next(1000, 9999).ToString();

            // generate password reset token (used later to reset password)
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var cacheKey = $"pwdreset:{phoneNumber}";
            var cacheValue = new { Code = code, Token = resetToken };

            await _cacheService.SetDataAsync(cacheKey, cacheValue, TimeSpan.FromMinutes(15));

            // TODO: integrate SMS provider to send the code to the user's phone
        }

        public async Task<bool> VerifyPasswordResetCodeAsync(string phoneNumber, string code)
        {
            var cacheKey = $"pwdreset:{phoneNumber}";
            var cached = await _cacheService.GetDataAsync(cacheKey);

            if (string.IsNullOrEmpty(cached))
                return false;

            try
            {
                var payload = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(cached);
                if (payload != null && payload.TryGetValue("Code", out var storedCode))
                {
                    return storedCode == code;
                }
            }
            catch
            {
                // ignore deserialization errors
            }

            return false;
        }

        public async Task ResetPasswordByPhoneAsync(string phoneNumber, string newPassword)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

            if (user is null)
                throw new InvalidOperationException("Invalid phone number.");

            var cacheKey = $"pwdreset:{phoneNumber}";
            var cached = await _cacheService.GetDataAsync(cacheKey);

            if (string.IsNullOrEmpty(cached))
                throw new InvalidOperationException("Reset token expired or invalid.");

            var payload = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(cached);
            if (payload == null || !payload.TryGetValue("Token", out var token))
                throw new InvalidOperationException("Reset token missing.");

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException(errors);
            }

            // remove cache entry after successful reset
            await _cacheService.SetDataAsync(cacheKey, "", TimeSpan.FromSeconds(1));
        }
    }
    }
