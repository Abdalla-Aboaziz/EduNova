using EduNova.Application.Common.Interfaces;
using EduNova.Application.Common.Results;
using EduNova.Application.Features.Devices.Commands;
using EduNova.Application.Interfaces;
using EduNova.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EduNova.Application.Features.Devices.Handlers;

public sealed class RegisterDeviceTokenCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IConfiguration configuration)
    : IRequestHandler<RegisterDeviceTokenCommand, Result>
{
    public async Task<Result> Handle(RegisterDeviceTokenCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId
            ?? configuration["EduNova:DemoStudentId"]
            ?? "demo-student-1";

        var deviceToken = await context.DeviceTokens
            .FirstOrDefaultAsync(d => d.Token == request.Token, cancellationToken);

        if (deviceToken is null)
        {
            deviceToken = new DeviceToken
            {
                UserId = userId,
                Token = request.Token,
                Platform = request.Platform,
                IsActive = true,
                LastUsedAt = DateTime.UtcNow
            };

            context.DeviceTokens.Add(deviceToken);
        }
        else
        {
            deviceToken.UserId = userId;
            deviceToken.Platform = request.Platform;
            deviceToken.IsActive = true;
            deviceToken.LastUsedAt = DateTime.UtcNow;
            deviceToken.UpdatedAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
