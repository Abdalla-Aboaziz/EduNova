using MediatR;
using Microsoft.AspNetCore.Http;

namespace EduNova.Application.Features.Lecture.Commands
{
    public record UploadLectureCommand(
      Guid SubjectId, string Title, string? Description,
      int Order, int DurationInSeconds, IFormFile Video, IFormFile? Thumbnail = null) : IRequest<Guid>;
}
