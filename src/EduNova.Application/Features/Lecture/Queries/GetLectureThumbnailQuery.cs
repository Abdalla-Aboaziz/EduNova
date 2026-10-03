using MediatR;

namespace EduNova.Application.Features.Lecture.Queries
{
    public record GetLectureThumbnailQuery(Guid Id) : IRequest<(FileStream? Stream, string ContentType, string FileName)>;
}
