namespace EduNova.Application.Features.Lecture.Responses
{
    public record DownloadResponse(Guid LectureId, string Title, int DurationInSeconds, DateTime DownloadedAt);
}
