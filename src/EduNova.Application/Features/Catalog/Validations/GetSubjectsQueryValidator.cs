using EduNova.Application.Common.Pagination;
using EduNova.Application.Features.Catalog.Queries;
using FluentValidation;

namespace EduNova.Application.Features.Catalog.Validations
{
    public class GetSubjectsQueryValidator : AbstractValidator<GetSubjectsQuery>
    {
        public GetSubjectsQueryValidator()
        {
            // Page >= 1 and 1 <= PageSize <= 50 (paging rules stay in one place).
            Include(new PagedRequestValidator());

            RuleFor(x => x.Search)
                .MaximumLength(100);
        }
    }
}
