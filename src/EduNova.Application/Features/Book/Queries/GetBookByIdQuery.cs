using EduNova.Application.Common.Results;
using EduNova.Application.Features.Book.Responses;
using MediatR;

namespace EduNova.Application.Features.Book.Queries;

public class GetBookByIdQuery(Guid id) : IRequest<Result<BookResponse>>
{
    public Guid Id { get; } = id;
}