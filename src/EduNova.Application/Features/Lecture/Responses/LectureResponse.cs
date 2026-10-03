namespace EduNova.Application.Features.Lecture.Responses
{
    public record LectureResponse(Guid Id, string Title, string? Description, int DurationInSeconds, int Order, bool IsMyList, bool IsDownLoaded);

}
