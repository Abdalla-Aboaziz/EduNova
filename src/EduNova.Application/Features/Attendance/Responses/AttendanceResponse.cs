using EduNova.Domain.Entities;

namespace EduNova.Application.Features.Attendance.Responses;

public class AttendanceResponse
{
    public Guid Id { get; set; }
    public Guid InstructorId { get; set; }
    public string InstructorName { get; set; } = string.Empty;
    public Guid? SubjectId { get; set; }
    public DateTime AttendanceDate { get; set; }
    public DateTime? CheckInAt { get; set; }
    public AttendanceStatus Status { get; set; }
}
