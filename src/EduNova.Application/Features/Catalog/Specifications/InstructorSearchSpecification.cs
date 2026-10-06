using EduNova.Application.Common.Specifications;
using EduNova.Domain.Entities;

namespace EduNova.Application.Features.Catalog.Specifications;

/// <summary>
/// Instructors for the Instructors screen: optional search by name.
/// Orders by name so paginated pages are stable. Pagination stays in
/// ToPagedResultAsync.
/// </summary>
public sealed class InstructorSearchSpecification : Specification<Instructor>
{
    public InstructorSearchSpecification(string? search)
    {
        if (!string.IsNullOrWhiteSpace(search))
            Where(i => i.FullName.Contains(search));

        OrderBy(i => i.FullName);
    }
}
