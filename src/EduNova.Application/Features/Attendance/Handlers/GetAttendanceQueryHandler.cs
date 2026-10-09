using EduNova.Application.Common.Interfaces;
using EduNova.Application.Common.Pagination;
using EduNova.Application.Common.Results;
using EduNova.Application.Features.Attendance.Queries;
using EduNova.Application.Features.Attendance.Responses;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EduNova.Application.Features.Attendance.Handlers;

public sealed class GetAttendanceQueryHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IConfiguration configuration)
    : IRequestHandler<GetAttendanceQuery, Result<PagedResult<AttendanceResponse>>>
{
    public async Task<Result<PagedResult<AttendanceResponse>>> Handle(
        GetAttendanceQuery request, CancellationToken cancellationToken)
    {
        var studentId = currentUser.UserId
            ?? configuration["EduNova:DemoStudentId"]
            ?? "demo-student-1";

        var query = context.AttendanceRecords
            .AsNoTracking()
            .Where(a => a.StudentId == studentId);

        if (request.InstructorId is not null)
        {
            query = query.Where(a => a.InstructorId == request.InstructorId);
        }

        if (request.From is not null)
        {
            query = query.Where(a => a.AttendanceDate >= request.From);
        }

        if (request.To is not null)
        {
            query = query.Where(a => a.AttendanceDate <= request.To);
        }

        var result = await query
            .OrderByDescending(a => a.AttendanceDate)
            .Select(a => new AttendanceResponse
            {
                Id = a.Id,
                InstructorId = a.InstructorId,
                InstructorName = a.Instructor.FullName,
                SubjectId = a.SubjectId,
                AttendanceDate = a.AttendanceDate,
                CheckInAt = a.CheckInAt,
                Status = a.Status
            })
            .ToPagedResultAsync(request, cancellationToken);

        return Result.Success(result);
    }
}
