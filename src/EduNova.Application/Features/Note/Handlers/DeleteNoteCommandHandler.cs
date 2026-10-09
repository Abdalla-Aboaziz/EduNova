using EduNova.Application.Common.Interfaces;
using EduNova.Application.Features.Note.Command;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Note.Handlers
{
    public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand, string>
    {


        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public DeleteNoteCommandHandler(IApplicationDbContext dbContext, ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _currentUserService = currentUser;
        }
        public async Task<string> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId ?? string.Empty;

            var note = await _dbContext.Notes
                .FirstOrDefaultAsync(
                    n => n.Id == request.Id &&
                         n.UserId == userId,
                    cancellationToken) ?? throw new Exception(
                    "Note not found or you do not have permission to delete this note.");
            _dbContext.Notes.Remove(note);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return "Note deleted successfully.";
        }
    }
}
