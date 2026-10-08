using MediatR;

namespace EduNova.Application.Features.Note.Command
{
    public class AddNoteCommand : IRequest<string>
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Color { get; set; }

    }
}
