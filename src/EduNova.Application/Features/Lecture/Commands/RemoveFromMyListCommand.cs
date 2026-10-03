using MediatR;

namespace EduNova.Application.Features.Lecture.Commands
{
    public record RemoveFromMyListCommand(Guid LectureId) : IRequest<bool>;

}
