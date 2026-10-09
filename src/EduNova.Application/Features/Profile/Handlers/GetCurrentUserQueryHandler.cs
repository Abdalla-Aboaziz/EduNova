using EduNova.Application.Common.Interfaces;
using EduNova.Application.Common.Results;
using EduNova.Application.Features.Profile.DTOs;
using EduNova.Application.Features.Profile.Queries;
using EduNova.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EduNova.Application.Features.Profile.Handlers;

public sealed class GetCurrentUserQueryHandler(
    UserManager<AppUser> userManager,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetCurrentUserQuery, Result<ProfileResponseDto>>
{
    public async Task<Result<ProfileResponseDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            return Result.Failure<ProfileResponseDto>(Error.Forbidden("PROFILE_UNAUTHORIZED", "User not authenticated"));
        }

        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure<ProfileResponseDto>(Error.NotFound("PROFILE_NOT_FOUND", "User not found"));
        }

        var dto = new ProfileResponseDto
        {
            Id = user.Id.ToString(),
            DisplayName = user.DisplayName,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber
        };

        return Result.Success(dto);
    }
}