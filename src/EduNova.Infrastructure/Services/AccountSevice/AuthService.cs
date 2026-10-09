using EduNova.Application.Contracts.Services;
using EduNova.Application.Features.Authentication.DTOs;
using EduNova.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduNova.Infrastructure.Services.AccountSevice
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public AuthService(
            UserManager<AppUser> userManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
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
    }
    }
