using EduNova.Application.Common.Interfaces;
using EduNova.Application.Features.Note.Command;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Note.Handlers
{
    public class AddNoteCommandHandler : IRequestHandler<AddNoteCommand, string>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public AddNoteCommandHandler(IApplicationDbContext dbContext, ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _currentUserService = currentUser;
        }



        public async Task<string> Handle(AddNoteCommand request, CancellationToken cancellationToken)
        {
            var userid = "1";// ToDo  _currentUserService.UserId;

            var existNote = await _dbContext.Notes.FirstOrDefaultAsync(x => x.Title == request.Title && x.UserId == userid);

            if (existNote != null)
            {
                return "Note with this title already exists.";
            }

            var note = new EduNova.Domain.Entities.Note
            {
                Title = request.Title,
                Content = request.Content,
                Color = (EduNova.Domain.Entities.NoteColor)request.Color,
                UserId = userid
            };

            _dbContext.Notes.Add(note);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return "Note added successfully.";
        }
    }
}
