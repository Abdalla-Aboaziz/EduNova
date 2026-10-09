using EduNova.Application.Common.Interfaces;
using EduNova.Application.Features.Note.Command;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Note.Handlers
{
    public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand, string>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public UpdateNoteCommandHandler(IApplicationDbContext dbContext, ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _currentUserService = currentUser;
        }

        public async Task<string> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId ?? string.Empty;

            var note = await _dbContext.Notes
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == userId, cancellationToken);

            if (note is null)
            {
                return "Note not found.";
            }

            var existNote = await _dbContext.Notes
                .FirstOrDefaultAsync(x => x.Title == request.Title && x.UserId == userId && x.Id != request.Id, cancellationToken);

            if (existNote != null)
            {
                return "Note with this title already exists.";
            }

            note.Title = request.Title;
            note.Content = request.Content;
            note.Color = (EduNova.Domain.Entities.NoteColor)request.Color;
            note.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return "Note updated successfully.";
        }
    }
}
