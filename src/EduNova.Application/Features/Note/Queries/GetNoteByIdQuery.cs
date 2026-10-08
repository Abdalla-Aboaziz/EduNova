using EduNova.Application.Features.Note.Responses;
using MediatR;

namespace EduNova.Application.Features.Note.Queries
{
    public class GetNoteByIdQuery : IRequest<NoteResponse>
    {
        public int Id { get; set; }
    }
}
