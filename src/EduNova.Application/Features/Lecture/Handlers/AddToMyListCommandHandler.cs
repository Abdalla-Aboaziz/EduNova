using EduNova.Application.Common.Interfaces;
using EduNova.Application.Features.Lecture.Commands;
using EduNova.Application.Interfaces;
using EduNova.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Lecture.Handlers
{
    public class AddToMyListCommandHandler : IRequestHandler<AddToMyListCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public AddToMyListCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<bool> Handle(AddToMyListCommand request, CancellationToken cancellationToken)
        {
            if (!await _context.Lectures.AnyAsync(l => l.Id == request.LectureId, cancellationToken)) return false;
            var userId = "101";// _currentUser.UserId;   // TODO: Implement user authentication and get the current user ID
            var exists = await _context.MyListItems
                .AnyAsync(m => m.LectureId == request.LectureId && m.UserId == userId, cancellationToken);
            if (!exists)
            {
                await _context.MyListItems.AddAsync(
                    new MyListItem
                    {
                        LectureId = request.LectureId,
                        UserId = userId
                    }
                );
                await _context.SaveChangesAsync(cancellationToken);
            }
            return true;
        }
    }
}
