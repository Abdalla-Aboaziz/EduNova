using EduNova.Application.Common.Specifications;
using EduNova.Domain.Entities;

namespace EduNova.Application.Features.Catalog.Specifications;

/// <summary>
/// Subjects for the Filter screen: optional year and semester filters plus
/// search by name or code. Orders by name so paginated pages are stable.
/// Pagination is NOT part of the specification — the handler pages the
/// result with ToPagedResultAsync.
/// </summary>
public sealed class SubjectSearchSpecification : Specification<Subject>
{
    public SubjectSearchSpecification(string? search, Guid? yearId, Guid? semesterId)
    {
        if (yearId is not null)
            Where(s => s.YearId == yearId);

        if (semesterId is not null)
            Where(s => s.SemesterId == semesterId);

        if (!string.IsNullOrWhiteSpace(search))
            Where(s => s.Name.Contains(search) || s.Code.Contains(search));

        OrderBy(s => s.Name);
    }
}
