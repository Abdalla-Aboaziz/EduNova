namespace EduNova.Application.Common.Results;

/// <summary>
/// Categories of failures a Result can carry.
/// Maps 1:1 to the HTTP status codes used by the global exception handler.
/// </summary>
public enum ErrorType
{
    /// <summary>Invalid input. Maps to 400/422.</summary>
    Validation,

    /// <summary>The requested resource does not exist. Maps to 404.</summary>
    NotFound,

    /// <summary>The request conflicts with the current state. Maps to 409.</summary>
    Conflict,

    /// <summary>Authenticated but not allowed. Maps to 403.</summary>
    Forbidden,

    /// <summary>Unexpected failure. Maps to 500.</summary>
    Failure
}
