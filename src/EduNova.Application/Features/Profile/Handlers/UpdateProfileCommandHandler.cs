using EduNova.Application.Common.Interfaces;
using EduNova.Application.Common.Results;
using EduNova.Application.Features.Profile.Commands;
using EduNova.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EduNova.Application.Features.Profile.Handlers;

public sealed class UpdateProfileCommandHandler(
    UserManager<AppUser> userManager,
    ICurrentUserService currentUserService)
    : IRequestHandler<UpdateProfileCommand, Result>
{
    public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            return Result.Failure(Error.Forbidden("PROFILE_UNAUTHORIZED", "User not authenticated"));
        }

        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(Error.NotFound("PROFILE_NOT_FOUND", "User not found"));
        }

        user.DisplayName = request.DisplayName;
        user.PhoneNumber = request.PhoneNumber;

        await userManager.UpdateAsync(user);

        return Result.Success();
    }
}