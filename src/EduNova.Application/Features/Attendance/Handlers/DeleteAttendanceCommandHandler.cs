using EduNova.Application.Common.Interfaces;
using EduNova.Application.Common.Results;
using EduNova.Application.Features.Attendance.Commands;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EduNova.Application.Features.Attendance.Handlers;

public sealed class DeleteAttendanceCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IConfiguration configuration)
    : IRequestHandler<DeleteAttendanceCommand, Result>
{
    public async Task<Result> Handle(DeleteAttendanceCommand request, CancellationToken cancellationToken)
    {
        var studentId = currentUser.UserId
            ?? configuration["EduNova:DemoStudentId"]
            ?? "demo-student-1";

        var record = await context.AttendanceRecords
            .FirstOrDefaultAsync(
                a => a.Id == request.Id && a.StudentId == studentId,
                cancellationToken);

        if (record is null)
        {
            return Result.Failure(Error.NotFound("AttendanceRecord", request.Id));
        }

        context.AttendanceRecords.Remove(record);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
