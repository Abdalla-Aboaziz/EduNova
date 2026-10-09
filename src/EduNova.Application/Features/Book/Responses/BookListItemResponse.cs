namespace EduNova.Application.Features.Book.Responses;

public record BookListItemResponse(
    Guid Id,
    string Title,
    string? Description,
    Guid? SubjectId,
    string? SubjectName,
    DateTime CreatedAt);