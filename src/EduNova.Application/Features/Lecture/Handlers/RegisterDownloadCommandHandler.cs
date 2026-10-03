using EduNova.Application.Common.Interfaces;
using EduNova.Application.Features.Lecture.Commands;
using EduNova.Application.Interfaces;
using EduNova.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Lecture.Handlers
{
    public class RegisterDownloadCommandHandler : IRequestHandler<RegisterDownloadCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public RegisterDownloadCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task<bool> Handle(RegisterDownloadCommand request, CancellationToken cancellationToken)
        {
            if (!await _context.Lectures.AnyAsync(l => l.Id == request.LectureId, cancellationToken))
                return false;
            var userId = "101";//_currentUserService.UserId; TODO: Implement user authentication and get the actual user ID
            var exists = await _context.DownloadedLectures.AnyAsync(ld => ld.LectureId == request.LectureId && ld.UserId == userId, cancellationToken);

            if (!exists)
            {
                var downloadedLecture = new DownloadedLecture
                {
                    LectureId = request.LectureId,
                    UserId = userId
                };
                _context.DownloadedLectures.Add(downloadedLecture);
                await _context.SaveChangesAsync(cancellationToken);
            }
            return true;
        }
    }
}
