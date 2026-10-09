using EduNova.Domain.Entities;

namespace EduNova.Application.Features.Events.Responses;

public class EventResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public EventType Type { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public string? Location { get; set; }
    public Guid? SubjectId { get; set; }
    public string? SubjectName { get; set; }
}
