using EduNova.Application.Common.Specifications;
using EduNova.Domain.Entities;

namespace EduNova.Application.Features.Catalog.Specifications;

/// <summary>
/// Offers of one instructor for the Dr. Details screen — its subject list.
/// Optionally narrowed by year (via the subject's curriculum placement) and
/// semester (when the instructor teaches it). Includes the subject so callers
/// reading entities get it loaded; handlers that project with Select right
/// after applying the specification can ignore the include (EF drops it).
/// Orders by subject name so pages are stable. No Skip/Take here.
/// </summary>
public sealed class InstructorOffersSpecification : Specification<Offer>
{
    public InstructorOffersSpecification(Guid instructorId, Guid? yearId = null, Guid? semesterId = null)
    {
        Where(o => o.InstructorId == instructorId);

        if (yearId is not null)
            Where(o => o.Subject.YearId == yearId);

        if (semesterId is not null)
            Where(o => o.SemesterId == semesterId);

        Include(o => o.Subject);

        OrderBy(o => o.Subject.Name);
    }
}
