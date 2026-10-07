using EduNova.Application.Common.Results;
using EduNova.Application.Features.Grades.Responses;
using MediatR;

namespace EduNova.Application.Features.Grades.Queries
{
    /// <summary>
    /// Chart data for the current student's Grade Sheet — like the sheet, it
    /// is always scoped to the resolved identity (no studentId parameter).
    /// </summary>
    public class GetGradeChartQuery : IRequest<Result<GradeChartResponse>>
    {
    }
}
