using EduNova.Application.Common.Pagination;
using EduNova.Application.Common.Results;
using EduNova.Application.Features.Lecture.Responses;
using MediatR;

namespace EduNova.Application.Features.Lecture.Queries
{
    public class GetMyListQuery : PagedRequest, IRequest<Result<PagedResult<MyListResponse>>>
    {

    }

}
