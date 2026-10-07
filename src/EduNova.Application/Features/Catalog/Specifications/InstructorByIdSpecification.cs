using EduNova.Application.Common.Specifications;
using EduNova.Domain.Entities;

namespace EduNova.Application.Features.Catalog.Specifications
{
    /// <summary>
    /// Single instructor fetch by id for the Dr. Details screen.
    /// The handler projects the subject list separately through
    /// InstructorOffersSpecification, so no includes are needed here.
    /// </summary>
    public sealed class InstructorByIdSpecification : Specification<Instructor>
    {
        public InstructorByIdSpecification(Guid id)
        {
            Where(i => i.Id == id);
        }
    }
}
