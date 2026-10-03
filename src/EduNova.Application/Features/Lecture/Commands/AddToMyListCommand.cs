using MediatR;

namespace EduNova.Application.Features.Lecture.Commands
{
    public record AddToMyListCommand(Guid LectureId) : IRequest<bool>;
}
