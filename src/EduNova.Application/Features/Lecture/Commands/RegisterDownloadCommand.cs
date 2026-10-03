using MediatR;

namespace EduNova.Application.Features.Lecture.Commands
{
    public record RegisterDownloadCommand(Guid LectureId) : IRequest<bool>;
}
