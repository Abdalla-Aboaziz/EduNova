using EduNova.Application.Common.Pagination;
using EduNova.Application.Features.Meetings.Queries;
using FluentValidation;

namespace EduNova.Application.Features.Meetings.Validations
{
    public class GetMeetingsQueryValidator : AbstractValidator<GetMeetingsQuery>
    {
        public GetMeetingsQueryValidator()
        {
            Include(new PagedRequestValidator());
        }
    }
}
