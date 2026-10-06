using EduNova.Application.Common.Pagination;
using EduNova.Application.Common.Results;
using EduNova.Application.Features.Meetings.Responses;
using MediatR;

namespace EduNova.Application.Features.Meetings.Queries
{
    public class GetMeetingsQuery : PagedRequest, IRequest<Result<PagedResult<MeetingResponse>>>
    {
    }
}
