using FluentValidation;

namespace EduNova.Application.Common.Pagination;

/// <summary>
/// Validates paging input. FluentValidation does not apply base-type validators
/// automatically, so paginated query validators pull these rules in explicitly
/// with Include(new PagedRequestValidator()).
/// </summary>
public class PagedRequestValidator : AbstractValidator<PagedRequest>
{
    public PagedRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, PagedRequest.MaxPageSize);
    }
}
