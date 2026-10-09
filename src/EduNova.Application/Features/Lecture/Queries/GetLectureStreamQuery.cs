using MediatR;

namespace EduNova.Application.Features.Lecture.Queries
{
    public class GetLectureStreamQuery(Guid Id) : IRequest<(FileStream? stream, string ContentType, string FileName)>
    {
        public Guid Id { get; } = Id;
    }
}
