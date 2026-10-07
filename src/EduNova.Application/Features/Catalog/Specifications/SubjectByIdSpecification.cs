using EduNova.Application.Common.Specifications;
using EduNova.Domain.Entities;

namespace EduNova.Application.Features.Catalog.Specifications
{
    /// <summary>
    /// Single subject fetch by id for the My Subject details screen.
    /// The handler projects year/semester/instructors from it, so no
    /// includes are needed.
    /// </summary>
    public sealed class SubjectByIdSpecification : Specification<Subject>
    {
        public SubjectByIdSpecification(Guid id)
        {
            Where(s => s.Id == id);
        }
    }
}
