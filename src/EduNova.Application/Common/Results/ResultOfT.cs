namespace EduNova.Application.Common.Results;

/// <summary>
/// A Result that carries a return value on success.
/// Create instances through Result.Success / Result.Failure — not directly.
/// </summary>
public class Result<T> : Result
{
    private readonly T? _value;

    protected internal Result(T? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    /// <summary>
    /// The carried value. Only readable when IsSuccess — accessing it on a
    /// failed result throws, so missing-value bugs surface immediately.
    /// </summary>
    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException(
            "Cannot access the value of a failed result. Check IsSuccess first.");
}
