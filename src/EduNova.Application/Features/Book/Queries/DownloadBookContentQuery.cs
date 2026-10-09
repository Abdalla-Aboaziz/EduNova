using MediatR;

namespace EduNova.Application.Features.Book.Queries;

public class DownloadBookContentQuery(Guid id) : IRequest<(byte[] fileContent, string contentType, string fileName)>
{
    public Guid Id { get; } = id;
}