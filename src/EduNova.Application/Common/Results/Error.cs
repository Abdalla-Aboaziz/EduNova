namespace EduNova.Application.Common.Results;

/// <summary>
/// Describes a failure inside a Result: a stable machine-readable code,
/// a human-readable message, the failure category, and optional
/// per-field validation details (field name -> list of messages).
/// </summary>
public sealed record Error(
    string Code,
    string Message,
    ErrorType Type,
    IDictionary<string, string[]>? Details = null)
{
    /// <summary>Sentinel used by successful results (no error).</summary>
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);

    public static Error NotFound(string code, string message)
        => new(code, message, ErrorType.NotFound);

    /// <summary>Same shape as the existing NotFoundException(entityName, key) message.</summary>
    public static Error NotFound(string entityName, object key)
        => new("NOT_FOUND", $"{entityName} with id '{key}' was not found.", ErrorType.NotFound);

    public static Error Conflict(string code, string message)
        => new(code, message, ErrorType.Conflict);

    public static Error Forbidden(string code, string message)
        => new(code, message, ErrorType.Forbidden);

    public static Error Failure(string code, string message)
        => new(code, message, ErrorType.Failure);

    /// <summary>Aggregates per-field validation failures (same dictionary shape as ValidationException.Errors).
    /// Code and status intentionally match the existing ValidationException so both paths
    /// produce an identical wire response (400 / VALIDATION_ERROR).</summary>
    public static Error Validation(IDictionary<string, string[]> details)
        => new("VALIDATION_ERROR", "One or more validation errors occurred.", ErrorType.Validation, details);
}
