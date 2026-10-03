using EduNova.Application.Common.Interfaces;
using EduNova.Application.Features.Lecture.Commands;
using EduNova.Application.Interfaces;
using EduNova.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EduNova.Application.Features.Lecture.Handlers
{
    public sealed class AddToMyListCommandHandler(
        IApplicationDbContext context, ICurrentUserService currentUser,
        ILogger<AddToMyListCommandHandler> logger)
        : IRequestHandler<AddToMyListCommand, bool>
    {
        public async Task<bool> Handle(AddToMyListCommand request, CancellationToken cancellationToken)
        {
            var userId = "101";// _currentUser.UserId;   // TODO: Implement user authentication and get the current user ID

            if (!await context.Lectures.AnyAsync(l => l.Id == request.LectureId, cancellationToken))
            {
                logger.LogWarning("Lecture not found. UserId: {UserId}, LectureId: {LectureId}", userId, request.LectureId);
                return false;
            }

            var exists = await context.MyListItems
                .AnyAsync(m => m.LectureId == request.LectureId && m.UserId == userId, cancellationToken);

            if (!exists)
            {
                await context.MyListItems.AddAsync(
                    new MyListItem
                    {
                        LectureId = request.LectureId,
                        UserId = userId
                    }
                );
                await context.SaveChangesAsync(cancellationToken);
            }

            return true;
        }
    }
}
