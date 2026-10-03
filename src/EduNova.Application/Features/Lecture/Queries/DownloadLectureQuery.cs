using MediatR;

namespace EduNova.Application.Features.Lecture.Queries
{
    public record DownloadLectureQuery(Guid LectureId) : IRequest<(byte[] FileContent, string ContentType, string FileName)?>;
}
