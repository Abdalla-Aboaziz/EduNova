using MediatR;

namespace EduNova.Application.Features.Lecture.Commands
{
    public record RemoveDownloadCommand(Guid LectureId) : IRequest<bool>;

}
