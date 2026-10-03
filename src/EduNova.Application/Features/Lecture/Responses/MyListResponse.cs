namespace EduNova.Application.Features.Lecture.Responses
{
    public record MyListResponse(Guid LectureId, string Title, int DurationInSeconds, DateTime AddedAt);
}
