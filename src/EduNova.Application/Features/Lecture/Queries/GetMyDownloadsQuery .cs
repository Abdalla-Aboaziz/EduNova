using EduNova.Application.Common.Pagination;
using EduNova.Application.Features.Lecture.Responses;
using MediatR;

namespace EduNova.Application.Features.Lecture.Queries
{
    public class GetMyDownloadsQuery : PagedRequest, IRequest<PagedResult<DownloadResponse>>
    {

    }
}
