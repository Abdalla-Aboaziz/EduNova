using MediatR;

namespace EduNova.Application.Features.Note.Command
{
    public class DeleteNoteCommand : IRequest<string>
    {
        public int Id { get; set; }
    }
}
