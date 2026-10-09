using EduNova.Application.Features.Profile.DTOs;

namespace EduNova.Application.Contracts.Services;

public interface IProfileService
{
    Task<ProfileResponseDto> GetCurrentUserAsync();
    Task UpdateCurrentUserAsync(UpdateProfileDto dto);
}