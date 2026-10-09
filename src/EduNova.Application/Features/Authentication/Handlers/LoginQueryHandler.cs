using EduNova.Application.Contracts.Services;
using EduNova.Application.Features.Authentication.DTOs;
using EduNova.Application.Features.Authentication.Queries;
using EduNova.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EduNova.Application.Features.Authentication.Handlers;

public sealed class LoginQueryHandler(
    UserManager<AppUser> userManager,
    ITokenService tokenService)
    : IRequestHandler<LoginQuery, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            throw new UnauthorizedAccessException(
                "Invalid email or password.");
        }

        var passwordValid = await userManager.CheckPasswordAsync(
            user,
            request.Password);

        if (!passwordValid)
        {
            throw new UnauthorizedAccessException(
                "Invalid email or password.");
        }

        var token = await tokenService.CreateTokenAsync(user);

        return new AuthResponseDto
        {
            DisplayName = user.DisplayName,
            Email = user.Email!,
            Token = token
        };
    }
}