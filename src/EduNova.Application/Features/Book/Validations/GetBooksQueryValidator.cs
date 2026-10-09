using EduNova.Application.Common.Pagination;
using EduNova.Application.Features.Book.Queries;
using FluentValidation;

namespace EduNova.Application.Features.Book.Validations
{
    public class GetBooksQueryValidator : AbstractValidator<GetBooksQuery>
    {
        public GetBooksQueryValidator()
        {
            Include(new PagedRequestValidator());

            RuleFor(x => x.Search)
                .MaximumLength(100);
        }
    }
}