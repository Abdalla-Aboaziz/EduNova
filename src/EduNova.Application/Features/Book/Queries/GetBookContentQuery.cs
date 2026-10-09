using MediatR;

namespace EduNova.Application.Features.Book.Queries;

public class GetBookContentQuery(Guid id) : IRequest<(Stream? stream, string ContentType, string FileName)>
{
    public Guid Id { get; } = id;
}