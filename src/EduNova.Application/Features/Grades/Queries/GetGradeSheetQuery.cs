using EduNova.Application.Common.Results;
using EduNova.Application.Features.Grades.Responses;
using MediatR;

namespace EduNova.Application.Features.Grades.Queries
{
    /// <summary>
    /// The current student's grade sheet with optional year/semester filters.
    /// There is deliberately NO studentId parameter — the sheet always belongs
    /// to the resolved identity (ICurrentUserService / demo fallback).
    /// </summary>
    public class GetGradeSheetQuery(Guid? yearId, Guid? semesterId)
        : IRequest<Result<GradeSheetResponse>>
    {
        public Guid? YearId { get; set; } = yearId;
        public Guid? SemesterId { get; set; } = semesterId;
    }
}
