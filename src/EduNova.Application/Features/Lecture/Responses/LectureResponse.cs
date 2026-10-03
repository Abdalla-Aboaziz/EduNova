namespace EduNova.Application.Features.Lecture.Responses
{
    public record LectureResponse(
      Guid Id, string Title, string? Description, int Order,
      int DurationInSeconds, bool HasThumbnail, bool IsInMyList, bool IsDownloaded);
}
