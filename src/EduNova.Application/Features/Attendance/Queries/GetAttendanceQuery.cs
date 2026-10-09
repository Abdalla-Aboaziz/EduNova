using EduNova.Application.Common.Pagination;
using EduNova.Application.Common.Results;
using EduNova.Application.Features.Attendance.Responses;
using MediatR;

namespace EduNova.Application.Features.Attendance.Queries;

public class GetAttendanceQuery : PagedRequest, IRequest<Result<PagedResult<AttendanceResponse>>>
{
    public Guid? InstructorId { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}
