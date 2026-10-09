using EduNova.Application.Common.Interfaces;
using EduNova.Application.Contracts.Services;
using EduNova.Application.Features.Profile.DTOs;
using EduNova.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EduNova.Infrastructure.Services.ProfileService;

public class ProfileService : IProfileService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ICurrentUserService _currentUserService;

    public ProfileService(UserManager<AppUser> userManager, ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _currentUserService = currentUserService;
    }

    public async Task<ProfileResponseDto> GetCurrentUserAsync()
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User not authenticated");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new UnauthorizedAccessException("User not found");
        }

        return new ProfileResponseDto
        {
            Id = user.Id.ToString(),
            DisplayName = user.DisplayName,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber
        };
    }

    public async Task UpdateCurrentUserAsync(UpdateProfileDto dto)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User not authenticated");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new UnauthorizedAccessException("User not found");
        }

        user.DisplayName = dto.DisplayName;
        user.PhoneNumber = dto.PhoneNumber;

        await _userManager.UpdateAsync(user);
    }
}