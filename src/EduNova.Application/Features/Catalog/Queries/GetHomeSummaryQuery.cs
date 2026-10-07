using EduNova.Application.Common.Results;
using EduNova.Application.Features.Catalog.Responses;
using MediatR;

namespace EduNova.Application.Features.Catalog.Queries
{
    /// <summary>
    /// Everything the Home screen needs in a single request. No input — the
    /// current term comes from configuration and the student from the
    /// authenticated user (demo fallback until Auth lands).
    /// </summary>
    public class GetHomeSummaryQuery : IRequest<Result<HomeSummaryResponse>>
    {
    }
}
