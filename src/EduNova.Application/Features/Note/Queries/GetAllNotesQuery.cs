using EduNova.Application.Common.Pagination;
using EduNova.Application.Features.Note.Responses;
using MediatR;

namespace EduNova.Application.Features.Note.Queries
{
    public class GetAllNotesQuery : PagedRequest, IRequest<PagedResult<NoteResponse>>
    {
    }

}
