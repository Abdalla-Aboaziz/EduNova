using EduNova.Application.Common.Results;
using EduNova.Application.Features.Catalog.Responses;
using MediatR;

namespace EduNova.Application.Features.Catalog.Queries
{
    /// <summary>Subject details for the My Subject screen.</summary>
    public class GetSubjectByIdQuery(Guid id) : IRequest<Result<SubjectDetailsResponse>>
    {
        public Guid Id { get; set; } = id;
    }
}
