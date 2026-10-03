using EduNova.Application.Common.Interfaces;
using EduNova.Application.Features.Lecture.Commands;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Lecture.Handlers
{
    public class RemoveDownloadCommandHandler : IRequestHandler<RemoveDownloadCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public RemoveDownloadCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<bool> Handle(RemoveDownloadCommand request, CancellationToken cancellationToken)
        {
            var userId = "101";//_currentUser.UserId; // TODO: Implement user authentication and get the current user ID
            var deleted = await _context.DownloadedLectures
                .Where(x => x.UserId == userId && x.LectureId == request.LectureId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }


    }
}
