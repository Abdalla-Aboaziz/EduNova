using EduNova.Application.Common.Interfaces;
using EduNova.Application.Features.Note.Queries;
using EduNova.Application.Features.Note.Responses;
using EduNova.Application.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Note.Handlers
{
    public class GetNoteByIdQueryHandler : IRequestHandler<GetNoteByIdQuery, NoteResponse>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public GetNoteByIdQueryHandler(IApplicationDbContext dbContext, ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _currentUserService = currentUser;
        }
        public async Task<NoteResponse> Handle(GetNoteByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = "1";//   ToDo _currentUserService.UserId;
            var note = await _dbContext.Notes.FirstOrDefaultAsync(n => n.Id == request.Id && n.UserId == userId);

            if (note is null)
            {
                throw new Exception("Note not found");
            }

            return note.Adapt<NoteResponse>();
        }
    }
}
