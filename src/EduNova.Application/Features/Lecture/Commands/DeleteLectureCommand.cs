using MediatR;

namespace EduNova.Application.Features.Lecture.Commands
{
    public record DeleteLectureCommand(Guid Id) : IRequest<bool>;
}
