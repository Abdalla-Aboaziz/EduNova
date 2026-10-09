using EduNova.Application.Contracts.Services;
using EduNova.Application.Features.Authentication.Commands;
using EduNova.Application.Features.Authentication.DTOs;
using EduNova.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EduNova.Application.Features.Authentication.Handlers;

public sealed class RegisterCommandHandler(
    UserManager<AppUser> userManager,
    ITokenService tokenService)
    : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await userManager.FindByEmailAsync(request.Email);

        if (existingUser is not null)
        {
            throw new InvalidOperationException(
                "User with this email already exists.");
        }

        var user = new AppUser
        {
            DisplayName = request.DisplayName,
            Email = request.Email,
            UserName = request.Email
        };

        var result = await userManager.CreateAsync(
            user,
            request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(
                ", ",
                result.Errors.Select(e => e.Description));

            throw new InvalidOperationException(errors);
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