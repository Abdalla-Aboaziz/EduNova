using EduNova.Application.Common.Results;
using EduNova.Application.Features.Notices.Responses;
using MediatR;

namespace EduNova.Application.Features.Notices.Queries;

public class GetNoticesQuery : IRequest<Result<List<NoticeResponse>>>
{
}
