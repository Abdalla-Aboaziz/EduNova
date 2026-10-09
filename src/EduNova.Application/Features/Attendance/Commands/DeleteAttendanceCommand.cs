using EduNova.Application.Common.Results;
using MediatR;

namespace EduNova.Application.Features.Attendance.Commands;

public class DeleteAttendanceCommand(Guid id) : IRequest<Result>
{
    public Guid Id { get; } = id;
}
