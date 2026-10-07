using EduNova.Application.Common.Pagination;
using EduNova.Application.Features.Catalog.Queries;
using FluentValidation;

namespace EduNova.Application.Features.Catalog.Validations
{
    public class GetInstructorsQueryValidator : AbstractValidator<GetInstructorsQuery>
    {
        public GetInstructorsQueryValidator()
        {
            Include(new PagedRequestValidator());

            RuleFor(x => x.Search)
                .MaximumLength(100);
        }
    }
}
