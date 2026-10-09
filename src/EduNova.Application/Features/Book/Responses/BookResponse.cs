namespace EduNova.Application.Features.Book.Responses;

public record BookResponse(
    Guid Id,
    string Title,
    string? Description,
    Guid? SubjectId,
    string? SubjectName,
    Guid FileId,
    string FileName,
    string ContentType,
    long FileSize);