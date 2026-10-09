using EduNova.Application.Common.Interfaces;
using EduNova.Application.Features.Note.Queries;
using EduNova.Application.Features.Note.Responses;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Note.Handlers
{
    public class GetAllNotesQueryHandler : IRequestHandler<GetAllNotesQuery, List<NoteResponse>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public GetAllNotesQueryHandler(IApplicationDbContext dbContext, ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _currentUserService = currentUser;
        }


        public async Task<List<NoteResponse>> Handle(GetAllNotesQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId ?? string.Empty;
            return await _dbContext.Notes
                .Where(n => n.UserId == userId)
                .Select(n => new NoteResponse
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    Color = (int)n.Color,
                    CreatedAt = n.CreatedAt,
                    UpdatedAt = n.UpdatedAt
                })
                .ToListAsync(cancellationToken);
        }
    }
}
