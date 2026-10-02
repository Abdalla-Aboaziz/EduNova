using FluentValidation;
using FluentValidation.Results;

namespace EduNova.Application.Common.Exceptions;

/// <summary>
/// Thrown when one or more FluentValidation rules fail.
/// Aggregates all validation errors into a dictionary for structured API responses.
/// </summary>
public class ValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException() : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }
}
