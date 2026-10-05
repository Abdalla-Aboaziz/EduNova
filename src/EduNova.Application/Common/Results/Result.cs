namespace EduNova.Application.Common.Results;

/// <summary>
/// Outcome of an operation: either success (optionally carrying a value via Result&lt;T&gt;)
/// or failure carrying an Error. Handlers return this instead of throwing
/// AppExceptions for expected failures, and controllers convert it with ToActionResult.
/// </summary>
public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException("A successful result cannot carry an error.");
        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException("A failing result must carry an error.");

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    /// <summary>Meaningful only when IsFailure is true (equals Error.None on success).</summary>
    public Error Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    /// <summary>Builds a Validation-type failure from per-field errors.</summary>
    public static Result ValidationFailure(IDictionary<string, string[]> details)
        => new(false, Error.Validation(details));

    public static Result<T> Success<T>(T value)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value), "A successful result cannot carry a null value.");
        return new(value, true, Error.None);
    }

    public static Result<T> Failure<T>(Error error) => new(default, false, error);
}
