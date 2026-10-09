using EduNova.Domain.Common;

namespace EduNova.Domain.Entities;

public sealed class AttendanceRecord : GuidKeyEntity
{
    public string StudentId { get; set; } = string.Empty;
    public Guid InstructorId { get; set; }
    public Guid? SubjectId { get; set; }
    public DateTime AttendanceDate { get; set; }
    public DateTime? CheckInAt { get; set; }
    public AttendanceStatus Status { get; set; }

    public Instructor Instructor { get; set; } = null!;
}
